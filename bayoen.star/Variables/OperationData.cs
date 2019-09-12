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

        private bool _isPPTOn;
        public bool IsPPTOn
        {
            get => this._isPPTOn;
            set
            {
                if (this._isPPTOn == value) return;

                if (value)
                {
                    this.MyName = Core.Memory.MyName;
                    this.MyID32 = Core.Memory.MyID32;
                    this._isPPTOn = true;
                }
                else
                {
                    this.Reset();
                    return;
                }
            }
            
        }

        public string MyName { get; set; }
        public int MyID32 { get; set; }

        public void Reset()
        {
            this.IsPPTOn = false;
            this.MyName = "##INVALID_NAME##";
            this.MyID32 = -1;
        }
    }
}
