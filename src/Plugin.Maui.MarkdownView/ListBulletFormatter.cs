using System.Globalization;

namespace Plugin.Maui.MarkdownView;

public static class ListBulletFormatter
{
    public static string GetListItemBullet(bool isOrderedList, int sequenceNumber, int listLevel, string listItemBulletOddCharacter, string listItemBulletEvenCharacter)
    {
        // 'listlevel' even or odd (list start at level 1 == odd)
        var isListOddLeveled = (listLevel % 2) != 0;

        if (!isOrderedList)
        {
            return (isListOddLeveled)
                ? listItemBulletOddCharacter
                : listItemBulletEvenCharacter;
        }

        const int aCharacterPosition = 97; // character 'a' position in ASCI table
        var sequenceNumberCharacterPosition = -1 + aCharacterPosition + sequenceNumber;

        var bullet = (isListOddLeveled)
            ? sequenceNumber.ToString()
            : Convert.ToChar(sequenceNumberCharacterPosition, CultureInfo.InvariantCulture).ToString();

        return $"{bullet}.";
    }
}