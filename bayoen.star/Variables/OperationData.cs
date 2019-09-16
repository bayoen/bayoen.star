using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class OperationData : FncJson
    {
        public OperationData()
        {
            this.Reset();            
        }

        //public bool IsNameOn { get; set; }
        private bool _isNameOn;
        public bool IsNameOn
        {
            get => this._isNameOn;
            set
            {
                if (this._isNameOn == value) return;

                if (value)
                {
                    this.MyName = Core.Memory.MyName;
                    this.MyID32 = Core.Memory.MyID32;
                    //this.MyID64 = Core.Memory.MyID64;

                    if (this.MyName.Length > 0)
                    {
                        if (this.MyID32 > -1)
                        {
                            this._isNameOn = true;
                        }
                    }
                    
                }
                else
                {
                    this._isNameOn = false;
                }                
            }

        }               

        public string MyName { get; private set; }
        public int MyID32 { get; private set; }
        //public long MyID64 { get; private set; }

        public void Reset()
        {
            this.IsNameOn = false;
            this.MyName = "";
            this.MyID32 = -1;
            //this.MyID64 = -1;
        }
    }
}
