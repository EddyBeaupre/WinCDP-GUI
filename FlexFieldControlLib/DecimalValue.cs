// Copyright (c) 2007 Michael Chapman
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace FlexFieldControlLib
{
   internal class DecimalValue : IValueFormatter
   {
      public virtual int MaxFieldLength
      {
         get { return string.Format( CultureInfo.InvariantCulture, "{0:d}", int.MaxValue ).Length - 1; }
      }

      public virtual string RegExString
      {
         get { return "[0-9]"; }
      }

      public virtual Size GetCharacterSize( Graphics g, Font font, CharacterCasing casing )
      {
         const int MeasureCharCount = 10;

         Size charSize = new Size( 0, 0 );

         for ( char c = '0'; c <= '9'; ++c )
         {
            Size newSize = TextRenderer.MeasureText( g, new string( c, MeasureCharCount ), font, new Size( 0, 0 ),
               _textFormatFlags );

            newSize.Width = (int)Math.Ceiling( (double)newSize.Width / (double)MeasureCharCount );

            if ( newSize.Width > charSize.Width )
            {
               charSize.Width = newSize.Width;
            }

            if ( newSize.Height > charSize.Height )
            {
               charSize.Height = newSize.Height;
            }
         }

         return charSize;
      }

      public virtual bool IsValidKey( KeyEventArgs e )
      {
         if ( e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9 )
         {
            if ( e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9 )
            {
               return false;
            }
         }

         return true;
      }

      public virtual int MaxValue( int fieldLength )
      {
         int result = 0;

         fieldLength = Math.Min( fieldLength, MaxFieldLength );
         string valueString = new String( '9', fieldLength );

         if ( Int32.TryParse( valueString, NumberStyles.Integer, CultureInfo.InvariantCulture, out result ) )
         {
            return result;
         }

         return 0;
      }

      public virtual int Value( string text )
      {
         if ( text == null )
         {
            return 0;
         }

         int result = 0;

         if ( Int32.TryParse( text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result ) )
         {
            return result;
         }

         return 0;
      }

      public virtual string ValueText( int value, CharacterCasing casing )
      {
         return value.ToString( CultureInfo.InvariantCulture );
      }

      private TextFormatFlags _textFormatFlags = TextFormatFlags.HorizontalCenter |
         TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
   }
}
