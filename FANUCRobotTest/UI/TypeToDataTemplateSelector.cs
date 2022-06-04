using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FANUCRobotTest.UI
{
    public class TypeToDataTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }
        public DataTemplate UnknownTypeTemplate { get; set; }
        public DataTemplate NullTypeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null || Resources == null)
            {
                if (NullTypeTemplate != null)
                    return NullTypeTemplate;
                return UnknownTypeTemplate;
            }

            var type = item.GetType();
            if (type == null)
                return NullTypeTemplate;
            if (Resources.Contains(type) || Resources.Contains(type.FullName))
            {
                var res = Resources[type] ?? Resources[type.FullName];
                if (res is BindableDataTemplate bt)
                    return bt.Template;
                return res as DataTemplate;
            }
            return UnknownTypeTemplate;
        }
    }

    [ContentProperty(nameof(Template))]
    public sealed class BindableDataTemplate
    {
        public DataTemplate Template { get; set; }
    }
}
