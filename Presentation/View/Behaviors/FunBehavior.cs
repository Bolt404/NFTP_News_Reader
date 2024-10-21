using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace WPF_MVVM_TEMPLATE.Presentation.View.Behaviors;

public class FunBehavior : Behavior<UIElement>
{
    protected override void OnAttached()
    {
        AssociatedObject.MouseEnter += OnClicked;
        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseEnter -= OnClicked;
        base.OnDetaching();
    }


    private int cnt = 0; 
    void OnClicked(object sender, EventArgs args)
    {
        UIElement source = sender as UIElement;
        if (source == null) return;
        cnt++;

        if (cnt >= 10)
        {
            cnt = 0;
        }
        
        SkewTransform skewTransform = new SkewTransform(cnt, cnt);
        source.RenderTransform = skewTransform; 
    }
    
}