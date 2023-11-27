using System;
using UnityEngine;

namespace BigNumbers
{
    public enum EBigNumberFormat { Basic, Scientific, Thousand, Letter }

// class that can scale number to a lot
    [Serializable]
    public class bn
    {
        // decimal part
        [SerializeField] private float _value;
        public float Value => _value;
    
        // e part
        [SerializeField] private int _exp;
        public int Exp => _exp;

    
        // Constructor
        public bn()
        {
            _value = 0;
            _exp = 0;
        }
        public bn(float value, int exp)
        {
            _value = value;
            _exp = exp;
        }

        public bn(bn value)
        {
            _value = value._value;
            _exp = value._exp;
        }

        // This Method is called after each operation to normalize the value and exp
        // the second part of this method take care that the float part don't keep to much information
        // big Number are not accurate after e10
        private void ReCount()
        {
            if (_value == 0)
                _exp = 0;
        
            while (_value >= 10 || (_value < 0 && _value <= -10))
            {
                _exp ++;

                _value /= 10;
            }

            while (_value < 1 && _value > 0 || (_value < 0 && _value > -1))
            {
                _exp--;
                _value *= 10;
            }
        
            if (_exp >= 9 || _exp <= -9)
            {
                int tempValue = (int) (_value * 100000000);
                _value = tempValue / 100000000f;
            }
        }

        #region Operator

        public static bn operator +(bn a, bn b)
        {
            // If the two number are more than 10^10 different, return the Biggest
            if (Math.Abs(a._exp - b._exp) >= 10) return Max(a, b);
            if (a == 0) return b;
            if (b == 0) return a;

            bn small = new bn(Min(a, b));
            bn big = new bn(Max(a, b));

            bn result = new bn(0, 0)
            {
                _exp = big._exp, 
                _value = small._value * Mathf.Pow(10, (small._exp - big._exp)) + big._value
            };
        
            result.ReCount();

            return result;
        }
    
        public static bn operator -(bn a, bn b)
        {
            // If the two number are more than 10^10 different
            if (Math.Abs(a._exp - b._exp) >= 10)
            {
                if (a > b)
                    return a;
                return -b;
            }

            bn result = new bn(0, 0);
            if(a._exp >= b._exp)
            {
                result._value = a._value - b._value * Mathf.Pow(10, (b._exp - a._exp));
                result._exp = a._exp;
            }
            else
            {
                result._value = a._value * Mathf.Pow(10, (a._exp - b._exp)) - b._value;
                result._exp = b._exp;
            }

            result.ReCount();

            return result;
        }

        public static bn operator -(bn a)
        {
            bn result = new bn(a);
            result._value *= -1;
            return result;
        }

        public static bn operator *(bn a, bn b)
        {
            bn result = new bn(a);
            result._exp += b._exp;
            result._value *= b._value;
        
            result.ReCount();
            return result;
        }

        public static bn operator /(bn a, bn b)
        {
            bn result = new bn(a);
            result._exp -= b._exp;
            result._value /= b._value;
        
            result.ReCount();

            return result;
        }

        public static bn operator *(bn a, int b)
        {
            bn result = new bn(a);
            result._value *= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator *(bn a, float b)
        {
            bn result = new bn(a);
            result._value *= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator *(bn a, double b)
        {
            bn result = new bn(a);
            result._value *= (float) b;
        
            result.ReCount();
            return result;
        }
        public static bn operator *(int b, bn a)
        {
            bn result = new bn(a);
            result._value *= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator *(float b, bn a)
        {
            bn result = new bn(a);
            result._value *= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator *(double b, bn a)
        {
            bn result = new bn(a);
            result._value *= (float) b;
        
            result.ReCount();
            return result;
        }

        public static bn operator /(bn a, float b)
        {
            bn result = new bn(a);
            result._value /= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator /(bn a, int b)
        {
            bn result = new bn(a);
            result._value /= b;
        
            result.ReCount();
            return result;
        }
        public static bn operator /(bn a, double b)
        {
            bn result = new bn(a);
            result._value /= (float)b;
        
            result.ReCount();
            return result;
        }

        #endregion

        #region BOOL LOGIC

        public static bool operator >(bn a, bn b)
        {
            if (a._value == 0 || b._value == 0)
            {
                return a._value > b._value;
            }
            if((a._value < 0 || b._value < 0) && (a._value > 0 || b._value > 0))
                return a._value > b._value;

            return a._exp > b._exp || a._exp == b._exp && a._value > b._value;
        }

        public static bool operator <(bn a, bn b)
        {
            if (a._value == 0 || b._value == 0)
            {
                return a._value < b._value;
            }

            return a._exp < b._exp || a._exp == b._exp && a._value < b._value;
        }

        public static bool operator ==(bn a, bn b)
        {
            return a._exp == b._exp && Math.Abs(a._value - b._value) < 0.01f;
        }

        public static bool operator !=(bn a, bn b)
        {
            return !(a == b);
        }

        public static bool operator >=(bn a, bn b)
        {
            return !(a < b);
        }

        public static bool operator <=(bn a, bn b)
        {
            return !(a > b);
        }
        protected bool Equals(bn other)
        {
            return _value.Equals(other._value) && _exp == other._exp;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((bn) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_value.GetHashCode() * 397) ^ _exp;
            }
        }

        #endregion

        #region Cast

        public static implicit operator bn(int a)
        {
            bn result = new bn(a, 0);
            result.ReCount();
            return result;
        }
        public static implicit operator bn(float a)
        {
            bn result = new bn(a, 0);
            result.ReCount();
            return result;
        }
        public static implicit operator bn(double a)
        {
            bn result = new bn((float) a, 0);
            result.ReCount();
            return result;
        }
        public static implicit operator bn(Vector2 a)
        {
            bn result = new bn( a.x, (int)a.y);
            result.ReCount();
            return result;
        }

        /// <summary>
        /// DO NOT USE THIS CAST IF YOU DON't Know THE VALUE;
        /// </summary>
        public static implicit operator float(bn a)
        {
            return a._value * Mathf.Pow(10, a._exp);
        }

        #endregion
    
        public static bn Max(bn a, bn b)
        {
            return a >= b ? a : b;
        }

        public static bn Min(bn a, bn b)
        {
            return a <= b ? a : b;
        }

        #region Notation

        // the class also handel the 3 or 4 Format to display big number on screen
        public override string ToString()
        {
            if (this < new bn(1,1))
            {
                return $"{(_value * Mathf.Pow(10, _exp)).ToString("0.0")}";
            }
            
            if (_exp < 6 && _exp >= 0)
            {
                return $"{Mathf.Round(_value * Mathf.Pow(10, _exp))}";
            }

            return $"{_value.ToString("0.00")} e{_exp}";
        }

        private string ToStringLetter()
        {
            if (_exp >= 64 && _exp != 100) return ToString(EBigNumberFormat.Thousand);
            string letter = "";

            int modulo = _exp % 3;
        
            switch (_exp - modulo)
            {
                case 3:
                    letter += "K";
                    break;
                case 6:
                    letter += "M";
                    break;
                case 9:
                    letter += "T";
                    break;
                case 12:
                    letter += "B";
                    break;
                case 15:
                    letter += "Q";
                    break;
                case 18:
                    letter += "QUI";
                    break;
                case 21:
                    letter += "SEX";
                    break;
                case 24:
                    letter += "SEP";
                    break;
                case 27:
                    letter += "OCT";
                    break;
                case 30:
                    letter += "NON";
                    break;
                case 33:
                    letter += "DEC";
                    break;
                case 36:
                    letter += "UND";
                    break;
                case 39:
                    letter += "DUOD";
                    break;
                case 42:
                    letter += "TRED";
                    break;
                case 45:
                    letter += "QD";
                    break;
                case 48:
                    letter += "QUID";
                    break;
                case 51:
                    letter += "SEXD";
                    break;
                case 54:
                    letter += "SEPD";
                    break;
                case 57:
                    letter += "OCTD";
                    break;
                case 60:
                    letter += "NOVD";
                    break;
                case 63:
                    letter += "VIG";
                    break;
                case 100:
                    letter += "Googol";
                    break;
            }

            return $"{(_value * (Mathf.Pow(10, modulo))).ToString("0.00")} {letter}";
        }
    
        private string ToStringThousand()
        {
        
            int modulo = _exp % 3;
            return $"{(_value * (Mathf.Pow(10, modulo))).ToString("0.00")} E {_exp - modulo}";
        }
    
        public string ToString(EBigNumberFormat format)
        {
            if (this < new bn(1,1))
            {
                return $"{(_value * Mathf.Pow(10, _exp)).ToString("0.0")}";
            }
            
            if (_exp < 6 && _exp >= 0)
            {
                return $"{Mathf.Round(_value * Mathf.Pow(10, _exp))}";
            }
        
            switch (format)
            {
                case EBigNumberFormat.Scientific:
                    return $"{_value:0.000} e{_exp}";
                case EBigNumberFormat.Thousand:
                    return ToStringThousand();
                case EBigNumberFormat.Letter:
                    return ToStringLetter();
                default:
                    return ToString();
            }
        }

        #endregion

        public static bn Abs(bn bn)
        {
            return new bn(Mathf.Abs(bn.Value), bn.Exp);
        }
    }
}