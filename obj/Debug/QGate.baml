   M S B A M L   `   `   `  ����N  JWPF_LogicSimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WPF_LogicSimulationWPF_LogicSimulation  5      9!clr-namespace:WPF_LogicSimulationWPF_LogicSimulation  5          WPF_LogicSimulation.QGate   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      1local!clr-namespace:WPF_LogicSimulation    5      /src!clr-namespace:WPF_LogicSimulation    5        � 	Resources  5
      &    WPF_LogicSimulation.CQBool2Brush   
bool2brush   True   False  bool2brush_endpoint*   $WPF_LogicSimulation.CQBoolVisibility  bool2visibility*   $WPF_LogicSimulation.CQBool2Thickness  bool2thickness��%�   &	        &	 )     &	 R     &	 e       5   
   
 � ���
 �   �6<     5   
   
 ����
 �����6I     5   
   6     5   
   6   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ itemscontrol_in��5      $��Right=�6O   ��60   �� *Pin_in+��5      �� 5      .R�5      ��  ��Name$ 	rectangle��6    -   5        
���   �$��10��5   �   $��10��6�   $��1��6�   ��63   �� *IsTrue+ �� 	Converter# [ 5      5      ��5      �� 5      .R�5      A�  A� Columns$ 1��6"   5       5"      �� 5#   
   
�������$��1��6!   ��61   �� *
IsSimulate+# [  ��6v   �� *
IsSimulate+# [ �� 5$   
   $��1��6.   $��Center=�6>   $��Center=�6Y   ��6   �� *
GateName+6Y   �� 5%   
   -   5        $ itemscontrol_out��5%      $��2��6Q   $
��Left=�6a   ��61   �� *	Pin_out+��5&      �� 5'      .R�5(      �� $ 	rectangle��6    -   5        
���   �$��10��5(   �   $��10��6�   $��1��6�   ��63   �� *IsTrue+# [ 6�   5)      ��5+      �� 5,      .R�5-      A� $ 1��6"   5.      50      51      52      