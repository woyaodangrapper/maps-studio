namespace Maps.Studio.Extension;

public static class VisualExtension
{
    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject? obj, string name = "") where T : FrameworkElement
    {
        if (obj is not null)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is not null and T && ((T)child).Name.Contains(name))
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child, name))
                    yield return childOfChild;
            }
    }

    public static IEnumerable<object> FindVisualChildren<T1, T2>(DependencyObject? obj)
       where T1 : FrameworkElement
       where T2 : FrameworkElement
    {
        if (obj is not null)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is not null and T1 or T2)
                    if (child is T1 a)
                        yield return a;
                    else
                    if (child is T2 b)
                        yield return b;

                foreach (object childOfChild in FindVisualChildren<T1, T2>(child))
                {
                    yield return childOfChild;
                }
            }
    }

    public static T? GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);

        while (parent != null)
        {
            if (parent is T t && (t.Name == name))
                return t;
            else
                parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }
}