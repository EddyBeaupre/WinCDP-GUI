using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlexFieldControlLib;



namespace TestFlexFieldControl
{
    class MACAddressControl : FlexFieldControl
    {
        public MACAddressControl()
        {
            // the format of this control is 'FF:FF:FF:FF:FF:FF'
            // set FieldCount first
            FieldCount = 6;
            // every field is 2 digits max
            SetMaxLength(2);
            // every separator is ':'...
            SetSeparatorText(":");
            // except for the first and last separators
            SetSeparatorText(0, String.Empty);
            SetSeparatorText(FieldCount, String.Empty);
            // the value format is hexadecimal
            SetValueFormat(ValueFormat.Hexadecimal);
            // use leading zeros for every field
            SetLeadingZeros(true);
            // use uppercase only
            SetCasing(CharacterCasing.Upper);
            // add ':' key to cede focus for every field
            KeyEventArgs e = new KeyEventArgs(Keys.OemSemicolon);
            AddCedeFocusKey(e);
            // this should be the last thing
            Size = MinimumSize;
        }
    }

    class IPV4AddressControl : FlexFieldControl
    {
        public IPV4AddressControl()
        {
            // the format of this control is '000.000.000.000'
            // set FieldCount first
            FieldCount = 4;
            // every field is 3 digits max
            SetMaxLength(3);
            // every separator is '.'...
            SetSeparatorText(".");
            // except for the first and last separators
            SetSeparatorText(0, String.Empty);
            SetSeparatorText(FieldCount, String.Empty);
            // the value format is decimal
            SetValueFormat(ValueFormat.Decimal);
            // Do not use leading zeros for every field
            SetLeadingZeros(false);
            // Decimal don't have case yet
            SetCasing(CharacterCasing.Normal);
            // add '.' key to cede focus for every field
            KeyEventArgs e = new KeyEventArgs(Keys.OemPeriod);
            AddCedeFocusKey(e);
            // set range from 0 t0 255 for every fields.
            SetRange(0, 255);
            // this should be the last thing
            Size = MinimumSize;
        }
    }

    class IPV6AddressControl : FlexFieldControl
    {
        public IPV6AddressControl()
        {
            // the format of this control is 'FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF'
            // set FieldCount first
            FieldCount = 8;
            // every field is 4 digits max
            SetMaxLength(4);
            // every separator is ':'...
            SetSeparatorText(":");
            // except for the first and last separators
            SetSeparatorText(0, String.Empty);
            SetSeparatorText(FieldCount, String.Empty);
            // the value format is hexadecimal
            SetValueFormat(ValueFormat.Hexadecimal);
            // use leading zeros for every field
            SetLeadingZeros(true);
            // use uppercase only
            SetCasing(CharacterCasing.Upper);
            // add ':' key to cede focus for every field
            KeyEventArgs e = new KeyEventArgs(Keys.OemSemicolon);
            AddCedeFocusKey(e);
            SetRange(0, 65535);
            // this should be the last thing
            Size = MinimumSize;
        }
    }
}
