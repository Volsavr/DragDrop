# DragDrop

![example workflow](https://github.com/Volsavr/DragDrop/actions/workflows/ci.yml/badge.svg)

## Description

WPF component that allows to extend UI elements of application with a drag-drop-functionality. Library supports MVVM pattern and allows to use drag-drop operations with touch. The idea of writing this library arose as a result of working with https://dragdrop.codeplex.com/ and the desire to expand its capabilities.

## Features

+ MVVM support (the logic for the drag and drop can be placed in a ViewModel. No code needs to be placed in codebehind)
+ Touch interaction support
+ Works with controls like `ItemsControl`
+ Ability to setup intersection method
+ Ability to display hints to give the user visual feedback of the operation in progress
+ Ability to use custom 'thumb' for dragging
+ Ability to setup angle for drag operation start 

## How to use

Declare drag drop container:
```
<dragDrop:DragDropContainer>
...
</dragDrop:DragDropContainer>
```

Declare drag drop source (Ellipse is used for demonstration purposes): 
```
<Ellipse x:Name="sourceEllipse" Height="80" Width="80" Fill="{Binding Color}"
         dragDrop:DragDropContainer.DragDropGroupName="dragDropGroup1"
         dragDrop:DragDropContainer.IsDraggable="True"
         dragDrop:DragDropContainer.IsDropTarget="False"
         dragDrop:DragDropContainer.SourceDropCommandParameter="{Binding}">
</Ellipse>
```

Declare drag drop target (Ellipse is used for demonstration purposes): 
```
 <Ellipse x:Name="targetEllipse" Fill="{Binding Color}"
         dragDrop:DragDropContainer.DragDropGroupName="dragDropGroup1"
         dragDrop:DragDropContainer.IsDraggable="False"
         dragDrop:DragDropContainer.IsDropTarget="True"
         dragDrop:DragDropContainer.SourceLeaveTargetCommand="{Binding SourceLeaveTargetCommand}"
         dragDrop:DragDropContainer.SourceReachTargetCommand="{Binding SourceReachTargetCommand}"
         dragDrop:DragDropContainer.TargetDropCommand="{Binding ItemDropCommand}"/>
```
**Where:**
`SourceReachTargetCommand` - the command that is executed when the source element of the drag reaches the target element
`SourceLeaveTargetCommand` - the command that is executed when the source element of the drag leaves the target element
`TargetDropCommand` - the command that is executed when customer drops source element on the target element

**Additional settings:**
`DragThumbTemplate, DragThumbContext` - allow to customize ui of dragged element
`MinDragAngle, MaxDragAngle` - allow to specify settings for drag start operation
`TargetCollisionMode` - allows specify intersection method
