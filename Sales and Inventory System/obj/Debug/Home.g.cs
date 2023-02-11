﻿#pragma checksum "..\..\Home.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "52B215DE0CB0460B73CFC0D2C50CA04CA822ACEABFEE9EB4915CEE3BE37E9022"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro;
using MahApps.Metro.Accessibility;
using MahApps.Metro.Actions;
using MahApps.Metro.Automation.Peers;
using MahApps.Metro.Behaviors;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Converters;
using MahApps.Metro.Markup;
using MahApps.Metro.Theming;
using MahApps.Metro.ValueBoxes;
using RootLibrary.WPF.Localization;
using Sales_and_Inventory_System;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Converters;
using Xceed.Wpf.Toolkit.Core;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Mag.Converters;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace Sales_and_Inventory_System {
    
    
    /// <summary>
    /// Home
    /// </summary>
    public partial class Home : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 47 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button user_btn;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label username_label;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button logout_btn;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button order_btn;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition item_details_column;
        
        #line default
        #line hidden
        
        
        #line 208 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label total_value;
        
        #line default
        #line hidden
        
        
        #line 228 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label total_item;
        
        #line default
        #line hidden
        
        
        #line 238 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid sales_grid_dropdown;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label sales_label;
        
        #line default
        #line hidden
        
        
        #line 253 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label income_total_main_daily;
        
        #line default
        #line hidden
        
        
        #line 254 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label income_total_main_weekly;
        
        #line default
        #line hidden
        
        
        #line 255 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label income_total_main_monthly;
        
        #line default
        #line hidden
        
        
        #line 256 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label income_total_main_yearly;
        
        #line default
        #line hidden
        
        
        #line 283 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button daily_income;
        
        #line default
        #line hidden
        
        
        #line 288 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label daily_income_total;
        
        #line default
        #line hidden
        
        
        #line 301 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button weekly_income;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label weekly_income_total;
        
        #line default
        #line hidden
        
        
        #line 319 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button monthly_income;
        
        #line default
        #line hidden
        
        
        #line 324 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label monthly_income_total;
        
        #line default
        #line hidden
        
        
        #line 337 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button yearly_income;
        
        #line default
        #line hidden
        
        
        #line 342 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label yearly_income_total;
        
        #line default
        #line hidden
        
        
        #line 369 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox search_textbox;
        
        #line default
        #line hidden
        
        
        #line 370 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button search_btn;
        
        #line default
        #line hidden
        
        
        #line 375 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button add_item;
        
        #line default
        #line hidden
        
        
        #line 378 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid item_data_grid;
        
        #line default
        #line hidden
        
        
        #line 510 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button show_item_details_btn;
        
        #line default
        #line hidden
        
        
        #line 512 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image show_item_details_image_btn;
        
        #line default
        #line hidden
        
        
        #line 517 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid item_preview;
        
        #line default
        #line hidden
        
        
        #line 542 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image item_image;
        
        #line default
        #line hidden
        
        
        #line 547 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock name_textblock;
        
        #line default
        #line hidden
        
        
        #line 552 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock description_textblock;
        
        #line default
        #line hidden
        
        
        #line 557 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock price_textblock;
        
        #line default
        #line hidden
        
        
        #line 562 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock stock_textblock;
        
        #line default
        #line hidden
        
        
        #line 566 "..\..\Home.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button edit_item_btn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Sales and Inventory System;component/home.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Home.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\Home.xaml"
            ((Sales_and_Inventory_System.Home)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.user_btn = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\Home.xaml"
            this.user_btn.Click += new System.Windows.RoutedEventHandler(this.user_btn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.username_label = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.logout_btn = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\Home.xaml"
            this.logout_btn.Click += new System.Windows.RoutedEventHandler(this.logout_btn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.order_btn = ((System.Windows.Controls.Button)(target));
            
            #line 146 "..\..\Home.xaml"
            this.order_btn.Click += new System.Windows.RoutedEventHandler(this.order_btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.item_details_column = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 7:
            this.total_value = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.total_item = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.sales_grid_dropdown = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.sales_label = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.income_total_main_daily = ((System.Windows.Controls.Label)(target));
            return;
            case 12:
            this.income_total_main_weekly = ((System.Windows.Controls.Label)(target));
            return;
            case 13:
            this.income_total_main_monthly = ((System.Windows.Controls.Label)(target));
            return;
            case 14:
            this.income_total_main_yearly = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            
            #line 259 "..\..\Home.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.sales_btn_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.daily_income = ((System.Windows.Controls.Button)(target));
            
            #line 283 "..\..\Home.xaml"
            this.daily_income.Click += new System.Windows.RoutedEventHandler(this.daily_sales_btn_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.daily_income_total = ((System.Windows.Controls.Label)(target));
            return;
            case 18:
            this.weekly_income = ((System.Windows.Controls.Button)(target));
            
            #line 301 "..\..\Home.xaml"
            this.weekly_income.Click += new System.Windows.RoutedEventHandler(this.weekly_sales_btn_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.weekly_income_total = ((System.Windows.Controls.Label)(target));
            return;
            case 20:
            this.monthly_income = ((System.Windows.Controls.Button)(target));
            
            #line 319 "..\..\Home.xaml"
            this.monthly_income.Click += new System.Windows.RoutedEventHandler(this.monthly_sales_btn_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            this.monthly_income_total = ((System.Windows.Controls.Label)(target));
            return;
            case 22:
            this.yearly_income = ((System.Windows.Controls.Button)(target));
            
            #line 337 "..\..\Home.xaml"
            this.yearly_income.Click += new System.Windows.RoutedEventHandler(this.yearly_sales_btn_Click);
            
            #line default
            #line hidden
            return;
            case 23:
            this.yearly_income_total = ((System.Windows.Controls.Label)(target));
            return;
            case 24:
            this.search_textbox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 25:
            this.search_btn = ((System.Windows.Controls.Button)(target));
            
            #line 370 "..\..\Home.xaml"
            this.search_btn.Click += new System.Windows.RoutedEventHandler(this.search_btn_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.add_item = ((System.Windows.Controls.Button)(target));
            
            #line 375 "..\..\Home.xaml"
            this.add_item.Click += new System.Windows.RoutedEventHandler(this.add_item_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            this.item_data_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 378 "..\..\Home.xaml"
            this.item_data_grid.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.window_MouseDown_item_grid);
            
            #line default
            #line hidden
            
            #line 378 "..\..\Home.xaml"
            this.item_data_grid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.item_data_grid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 30:
            this.show_item_details_btn = ((System.Windows.Controls.Button)(target));
            
            #line 510 "..\..\Home.xaml"
            this.show_item_details_btn.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.window_MouseDown_item_grid);
            
            #line default
            #line hidden
            
            #line 510 "..\..\Home.xaml"
            this.show_item_details_btn.Click += new System.Windows.RoutedEventHandler(this.show_item_details_btn_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.show_item_details_image_btn = ((System.Windows.Controls.Image)(target));
            return;
            case 32:
            this.item_preview = ((System.Windows.Controls.Grid)(target));
            return;
            case 33:
            
            #line 523 "..\..\Home.xaml"
            ((System.Windows.Controls.Grid)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.window_MouseDown_item_grid);
            
            #line default
            #line hidden
            return;
            case 34:
            
            #line 525 "..\..\Home.xaml"
            ((System.Windows.Controls.Grid)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.window_MouseDown_item_grid);
            
            #line default
            #line hidden
            return;
            case 35:
            this.item_image = ((System.Windows.Controls.Image)(target));
            return;
            case 36:
            this.name_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 37:
            this.description_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 38:
            this.price_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 39:
            this.stock_textblock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 40:
            this.edit_item_btn = ((System.Windows.Controls.Button)(target));
            
            #line 566 "..\..\Home.xaml"
            this.edit_item_btn.Click += new System.Windows.RoutedEventHandler(this.edit_item_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 28:
            
            #line 435 "..\..\Home.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.view_item_btn_Click);
            
            #line default
            #line hidden
            break;
            case 29:
            
            #line 465 "..\..\Home.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.delete_item_btn_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

