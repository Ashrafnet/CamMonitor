using System;
using System.Collections.Generic;
using System.Text;

namespace CamMonitor
{
    class clsItems
    {
    }
    public class ItemData
    {
        private string m_Display;
        private string  m_Value;
        public ItemData(string Display, string  Value)
        {
            m_Display = Display;
            m_Value = Value;
        }
        public string Display
        {
            get { return m_Display; }
        }
        public string  Value
        {
            get { return m_Value; }
        }
    }

}
