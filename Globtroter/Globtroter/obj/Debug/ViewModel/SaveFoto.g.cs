﻿

#pragma checksum "C:\Users\agua\Globtroter\Globtroter\Globtroter\ViewModel\SaveFoto.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EC7C87BA3B0AC2758D57C420B6AE9340"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Globtroter.ViewModel
{
    partial class SaveFoto : global::Globtroter.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 75 "..\..\ViewModel\SaveFoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.OnButtonClick_SaveChanges;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 76 "..\..\ViewModel\SaveFoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.OnButtonClick_CancelChanges;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 62 "..\..\ViewModel\SaveFoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ComboBoxSubgroup_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 37 "..\..\ViewModel\SaveFoto.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


