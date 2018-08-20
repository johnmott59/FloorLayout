
/*
 * For draw operations that have different kinds of handles this enum can be used to identify them
 */
public enum eHandleType
{
    MoveHandle,
    ResizeHandle,
    EdgeHandle,
    VertexHandle,
}

public enum eMouseState
{
    Nothing,
    MouseDown,
    MouseMovedWhileDown
}

public enum eOperationStatus
{
    OK,
    NoLayerSelected,
    NoEdgeSelected,
    NoEdgesDefined,
    NoVertexSelected,
    EdgeNotWideEnoughForOperation,
    MustBeTwoConnectingEdgesForOperation
}

public enum eMouseDownCapture
{
    Nothing,
    VertexHandle,
    EdgeHandle
}

