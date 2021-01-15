using System;
using System.Collections.Generic;
using System.Linq;

namespace Jgs.UI.Components.SpinEditCore
{
    /// <summary>
    /// Builder pro kontrolu povolených znaků v numerickém vstupu.
    /// Nepodporuje měnu
    /// </summary>
    public class NumericAllowedCharsBuilder
    {
        private readonly List<string> m_allowedChars = new List<string>(13);
        private string m_decimalSeparator;
        private string m_thousandsSeparator;
        private string m_minusSign;
        private string m_textToCompare;
        private int? m_caretIndex;
        private bool m_duplicityCheck;

        /// <summary>
        /// Zjednodušený přístup na nastavený oddělovač desetin
        /// </summary>
        public static string DecimalSeparator => System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>
        /// Zjednodušený přístup na nastavený oddělovač tisíců
        /// </summary>
        public static string ThousandsSeparator => System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;

        /// <summary>
        /// Povolené znaky budou/nebudou obsahovat oddělovač desetin
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <returns></returns>
        public NumericAllowedCharsBuilder WithDecimalSeparator(bool isAllowed = true)
        {
            if (isAllowed)
            {
                m_decimalSeparator = DecimalSeparator;
            }

            return this;
        }

        /// <summary>
        /// Povolené znaky budou/nebudou obsahovat znaménko mínus. Povolený znak mínus se musí objevit pouze na počátku textu. Předává se index,
        /// kam přijde daný znak v porovnávaném textu předaném v <see cref="WithTextToCompare"/>
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <param name="currentCaretIndex"></param>
        /// <returns></returns>
        public NumericAllowedCharsBuilder WithMinusSign(bool isAllowed = true, int? currentCaretIndex = null)
        {
            if (isAllowed)
            {
                m_minusSign = "-";
                m_caretIndex = currentCaretIndex;
            }

            return this;
        }

        /// <summary>
        /// Povolené znaky budou/nebudou obsahovat oddělovač tisícin
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <returns></returns>
        public NumericAllowedCharsBuilder WithThousandSeparator(bool isAllowed = true)
        {
            if (isAllowed)
            {
                m_thousandsSeparator = ThousandsSeparator;
            }

            return this;
        }

        /// <summary>
        /// Text k porovnaní pro operace <see cref="WithDuplicityCheck"/> a <see cref="WithMinusSign"/>
        /// </summary>
        /// <param name="textToCompare"></param>
        /// <returns></returns>
        public NumericAllowedCharsBuilder WithTextToCompare(string textToCompare)
        {
            m_textToCompare = textToCompare;
            return this;
        }

        /// <summary>
        /// Povolené znaky, které obsahují oddělovač desetin a znaménko mínus se budou
        /// kontrolovat na duplicity v textu předaném v <see cref="WithTextToCompare"/>
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <returns></returns>
        public NumericAllowedCharsBuilder WithDuplicityCheck(bool isAllowed = true)
        {
            m_duplicityCheck = isAllowed;
            return this;
        }

        /// <summary>
        /// Metoda vrací příznak, zda je zadný znak povolen
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsAllowed(string text)
        {
            m_allowedChars.Clear();
            for (var i = 0; i < 10; i++)
            {
                m_allowedChars.Add($"{i}");
            }

            if (!string.IsNullOrEmpty(m_decimalSeparator))
            {
                m_allowedChars.Add(m_decimalSeparator);
            }

            if (!string.IsNullOrEmpty(m_thousandsSeparator))
            {
                m_allowedChars.Add(m_thousandsSeparator);
            }

            if (!string.IsNullOrEmpty(m_minusSign) && (!m_caretIndex.HasValue || m_caretIndex.Value == 0))
            {
                m_allowedChars.Add(m_minusSign);
            }

            var result = m_allowedChars.Contains(text);

            return result && !HasDuplicities(text, m_decimalSeparator, m_minusSign);
        }

        /// <summary>
        /// Kontrola na duplicity
        /// </summary>
        /// <param name="text"></param>
        /// <param name="compareChars"></param>
        /// <returns></returns>
        private bool HasDuplicities(string text, params string[] compareChars)
        {
            var result = false;
            if (m_duplicityCheck && !string.IsNullOrEmpty(m_textToCompare) && compareChars.Contains(text))
            {
                result = m_textToCompare.Contains(text);
            }

            return result;
        }
    }
}