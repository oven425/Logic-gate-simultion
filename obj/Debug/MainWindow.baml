   M S B A M L   `   `   `  ����N  JWPF_LogicSimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WPF_LogicSimulationWPF_LogicSimulation  5   	   $    WPF_LogicSimulation.MainWindow   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5   	   1local!clr-namespace:WPF_LogicSimulation    5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � -   5         �Name$
 grid��5	       � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� -   5         ��Name$ combobox_gate��5       �� SelectedIndex$ 0��6.   .@�5      �� .��6   NOT6$   �� 5      .��6   AND6$   �� 5      .��6   OR6#   �� 5      .��6   Switch6'   �� 5      .��6   LED6$   5      �� 5      -   5         ��Name$ button_addgate��5      .��6J   Add6O   �� 5      -   5         ��Name$ textbox_savename��5      $��QQ��60   $��60��6:   �� 5      -   5        $ button_save��5      .��6D   Save6J   �� 5      -   5        $ button_load��5      .��6D   Load6J   �� 5      -   5        $ button_clear��5      .��6F   Clear6M   d� 5      -	   5         d�Name$ togglebutton_simulation��5      .��6b   Simulatione6o   5      �� 5   
   -
   5        	 ��Name$	 canvas��5      
������ $��1��6:   5      �� 5    
   $��2��64   ��6   �� *	SaveFiles+��5!      �� 5"      .R�5#      �� 
��� � �$��1q�61   ._�5$      �� .+�5%      �� $��50��6@   ��6$   �� *
Snapshot+
 � ToolTip
 5&   "   �� 5'   &   ��6,   �� *
Snapshot+5)      �� 5*      $��QQ��6(   $��Center=�62   5+      5,      5-      ��5/      �� 50      .R�51      �� $ 
Horizontal=�6!   52      54      55      56      