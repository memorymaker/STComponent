using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DAC
{
    public class ProcedureParameter
    {
        public object Value;
        public object DataType;
        public ParameterDirection? Direction;

        public ProcedureParameter(object _Value, object _DataType = null, ParameterDirection? _Direction = null)
        {
            Value = _Value;
            DataType = _DataType;
            Direction = _Direction;
        }
    }
}
