namespace Microsoft.VisualStudioUI.Options.Models
{
    public delegate string ItemDisplayStringFunc<in TItem>(TItem? item) where TItem : class;
    
    public static class DisplayableItemsUtil
    {
        public static string ItemDisplayStringFromToString<TItem>(TItem? item) where TItem : class
        {
            if (item == null)
                return "";

            return item.ToString();
        }
        
        public static TItem? FindMatch<TItem>(TItem[] items, string? displayString, ItemDisplayStringFunc<TItem> itemDisplayStringFunc) where TItem : class
        {
            if (displayString == null)
                return null;
            
            foreach (TItem item in items)
            {
                string itemDisplayString = itemDisplayStringFunc(item);
                if (itemDisplayString.Equals(displayString))
                    return item;
            }
            return null;
        }
        
        public static string? FindMatch(string[] items, string? displayString)
        {
            if (displayString == null)
                return null;
            
            foreach (string item in items)
            {
                if (item.Equals(displayString))
                    return item;
            }
            return null;
        }
    }
}