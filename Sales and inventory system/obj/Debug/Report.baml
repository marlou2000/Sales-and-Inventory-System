   M S B A M L   `   `   `  ����U  QSales and Inventory System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Sales_and_Inventory_SystemSales_and_Inventory_System  5      '    !Sales_and_Inventory_System.Report   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Sales_and_Inventory_System     5        N� Title$  Report��5	      .��5
      � .+�5   
   �� $��img/Background.jpg��6    K� ZIndex$ 0��6,   $��2��6=   $
��Fill=�6Q    J� Opacity$ 0.175Q�6`   � 5   
   $��20,20,20,20q�6    � RowDefinitions 5      �� 5      $��21 �6    6.   �� 5      �� 5      $��21 �6    6.   .+�5      � $��1��6   .+�5      � $
��Left=�6   $	��Top=�62   $��32��6J   $	��210��6V   $��0,6,0,0q�6b   .+�5      �� -   5         ��Name$ sales_report_combo_box��5      .@�5      �� $��Daily��6'   69   �� 5      $��Weekly��6'   6:   �� 5      $��Monthly��6'   6;   �� 5      $��Yearly��6'   6:   5      5      &   System.Windows.Controls.DataGrid  5      -   5          Name$ report_grid��5   ]  $��1��6S    �� AlternationCount$ 2��6`      AlternatingRowBackground �#A7FAB1   HeadersVisibility$ Column=�6�   $��Center=�6�   	   CanUserAddRows	 .  
   CanUserReorderColumns
 .     CanUserResizeColumns .     AutoGenerateColumns .     CanUserResizeRows .  
�������
������ $
��Left=�6�     HorizontalGridLinesBrush 6�  Z�    VerticalGridLinesBrush 6�  Z�  � 	Resources 5       �� Color*  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%h  (��       5    *   *1/SystemColors.InactiveSelectionHighlightBrushKey+)(��      5!   *   *53SystemColors.InactiveSelectionHighlightTextBrushKey+)(�� =     5"   *   * SystemColors.HighlightBrushKey+)'  [     �� 5       $
 Blue��6m   �� 5!      $ White��6q   �� 5"      $
 Blue��6\   �� 5#      "  6    .�5$      ��  � ��$ 0,0,0,0q�6@   �� 5%       � ��
 ���� 6<   �� 5&       �@�FocusVisualStyle 6A   Z�  5'      P� 5(   "    �@ 
IsSelected . . �5)   &   ��  � ��$ 0,0,0,0q�6H   �� 5*   &    � ��
 ���� 6D   5+   #   5-      5.         ColumnHeaderStyle 51      �� 52      " ��6    .�53      ��  � ��$ Century Gothic)�6;   �� 54       � ��
 �����6;   �� 55       � �� �#1392226;   �� 56       � ��$ 10, 5, 5, 5q�68   �� 57       � ��
 ���� 6<   �� 58       � ��$ 0q�6@   59         	CellStyle 5<      �� 5=      "  6    .�5>      ��  �@�Cursor$
 Hand��67   �� 5?       � ��$ 0q�6@   �� 5@       � ��
 ���� 6<   �� 5A       �@�FocusVisualStyle 6A   Z� �� 5B       � �� 5C   "   �� 5D   &    �� 
TargetType"  66   .R�5E   *   ��  �� Padding# z��61   #��z��6U   #��z��6�   #��z��6�    J� SnapsToDevicePixels . ._�5F   .   �� # z 6?   5G   +   5H   '   5J      5K         Columns 5N      0  *System.Windows.Controls.DataGridTextColumn  5O         SalesMargin1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6-   ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .     Header$  Order ID��6�   !   Width5  /System.Windows.Controls.DataGridLengthConverter$
! Auto 6�   "   Binding" 6k   �� *order_total_id+6�     5P      # [  6-    . $  Customer Name��6�   $! * 6�   " 6k   �� *customer_name+6�     5Q      # [  6-    . $  Item Quantity��6�   #   MinWidth$# 80Q�6�   $
! Auto 6�   " 6k   �� *total_item_quantity+6�     5R      # [  6-    . $  
Total Cost��6�   $# 80Q�6�   $
! Auto 6�   " 6k   �� *
total_cost+6�     5S      # [  6-    . $  Date Ordered��6�   $# 80Q�6�   $
! Auto 6�   " 6k   �� *date_ordered+$ �� StringFormat$$ 
yyyy-MM-dd��6�   5U      5V      5W      5X      5Y      