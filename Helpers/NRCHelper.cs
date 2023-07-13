namespace DOTP_BE.Helpers
{
    public class NRCHelper
    {
        //Change NRC Myanmar To English Digit
        public static string ChangeNRC_MyanToEnglish(string nrcmmStr)
        {
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['၀'] = '0',
                ['၁'] = '1',
                ['၂'] = '2',
                ['၃'] = '3',
                ['၄'] = '4',
                ['၅'] = '5',
                ['၆'] = '6',
                ['၇'] = '7',
                ['၈'] = '8',
                ['၉'] = '9',
                ['0'] = '0',
                ['1'] = '1',
                ['2'] = '2',
                ['3'] = '3',
                ['4'] = '4',
                ['5'] = '5',
                ['6'] = '6',
                ['7'] = '7',
                ['8'] = '8',
                ['9'] = '9'
            };
            //foreach (var item in nrcmmStr)
            //{
            //    nrcmmStr = nrcmmStr.Replace(item, LettersDictionary[item]);
            //}
            foreach(var item in nrcmmStr)
            {
                if(LettersDictionary.ContainsKey(item))
                {
                    nrcmmStr = nrcmmStr.Replace(item, LettersDictionary[item]);
                }
            }
            return nrcmmStr;
        }
    }
}
