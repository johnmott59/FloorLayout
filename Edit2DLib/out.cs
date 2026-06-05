public class Edit2DBase
{
        public eMouseDownCapture BaseMouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            // Indicate new mouse state
            MouseState = eMouseState.MouseDown;
            // Call the override mouse down method

            return MouseDown(ScreenMouseX, ScreenMouseY);
        }


        public bool BaseMouseMove(int ScreenMouseX, int ScreenMouseY)
        {
            // Compute the screen delta x and delta y based on last mouse position
            int ScreenDeltaX = ScreenMouseX - LastScreenMousePosition.X;
            int ScreenDeltaY = ScreenMouseY - LastScreenMousePosition.Y;

            // Update the last mouse position
            LastScreenMousePosition = new Point(ScreenMouseX, ScreenMouseY);

            if (MouseState == eMouseState.MouseDown)
            {
                MouseState = eMouseState.MouseMovedWhileDown;
            }

            /*
             * Our action takes place if the mouse is down while its being moved.
             */
            if (MouseState != eMouseState.MouseMovedWhileDown) return false;

            if (MouseMove(MouseState, ScreenMouseX, ScreenMouseY,ScreenDeltaX,ScreenDeltaY)) return true;

            // Draw set didn't change things, we can scroll

            int WorldDeltaX = (int)((float)ScreenDeltaX * CurrentZoom);
            int WorldDeltaY = (int)((float)ScreenDeltaY * CurrentZoom);

            WorldPointScrollX -= WorldDeltaX;
            WorldPointScrollY -= WorldDeltaY;

            // After a scroll we need to repaint

            return true;
        }



        public void BaseMouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            // pass this to the draw operation object
            MouseUp(ScreenMouseX, ScreenMouseY);
            /*
             * Reset mouse state and cursor
             */
            MouseState = eMouseState.Nothing;
        }


        public void Clear(string color)
        {
#if DOTNET
            oGraphics.Clear(Color.White);
#else
            this.context.fillStyle = color;
            this.context.fillRect(0, 0,
                this.context.canvas.clientWidth,
                this.context.canvas.clientHeight);
#endif
        }


        public Edit2DBase()
        {

            LastScreenMousePosition = new Point(0, 0);
            MouseState = eMouseState.Nothing;

            WorldPointScrollX = 0;
            WorldPointScrollY = 0;

            CurrentZoom = 1;

            GridSize = 1;
        }


        public void DrawAxis()
        {

            PointF XaxisFrom = this.W2S(-100, 0);
            PointF XaxisTo = this.W2S(100, 0);

            DrawLine("rgba(150,150,150,.5)", (float).5, XaxisFrom, XaxisTo);

            PointF YaxisFrom = this.W2S(0, -100);
            PointF YaxisTo = this.W2S(0, 100);

            DrawLine("rgba(150,150,150,.5)", (float).5, YaxisFrom, YaxisTo);

        }
      

        


        public void DrawCircle(string color, float Radius, float CenterX, float CenterY)
        {
#if DOTNET
            Pen p = new Pen(Brushes.Blue, 1);
            RectangleF rf = new RectangleF(CenterX - Radius, CenterY - Radius, Radius, Radius);
            oGraphics.DrawEllipse(p, rf);
#else
            this.context.save();

            this.context.lineWidth = 1;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.arc(CenterX, CenterY, Radius, 0, 2 * Math.PI);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }



        // There is a W3C ellipse function for the context but it doesn't compile, not sure of browser support
        public void DrawEllipse(string color, float Width, float Height, float CenterX, float CenterY)
        {
            // Don't allow it to become negative or 0
            if (Width < 0) Width = 1;
            if (Height < 0) Height = 1;
#if DOTNET
#else
        this.context.save();
        this.context.beginPath();
       
        this.context.translate(CenterX, CenterY);
        var scalex = 1;
        var scaley = 1;
        if (Width > Height) {
            scalex = Width / Height;
        }
        if (Height > Width) {
            scaley = Height / Width;
        }
        this.context.scale(scalex, scaley);
       
        this.context.arc(0, 0, Math.min(Width, Height), 0, 2 * Math.PI, false);
            
        // we call restore before calling stroke so that the line drawing of the ellipse isn't scaled, only the dimensions
        this.context.restore();
        this.context.lineWidth = 1;
        this.context.strokeStyle = color;
        this.context.stroke();
#endif
        }



        protected void DrawGrid()
        {
            if (this.GridSize == 1) return;

            int clientWidth = PictureBoxWidth;
            int clientHeight = PictureBoxHeight;
            int GridSize =this.GridSize;

            PointF ScreenCenter = new PointF(clientWidth / 2, clientHeight / 2);
            PointF WorldCenter = this.S2W(ScreenCenter.X, ScreenCenter.Y);

            // Adjust the world coordinates to the grid size
            WorldCenter.X = RoundToGrid((int)WorldCenter.X);
            WorldCenter.Y = RoundToGrid((int)WorldCenter.Y);

            int HorizontalLines = clientHeight / this.GridSize / 2;
            int HorizontalWidth = clientWidth / 2;

            for (int i = -HorizontalLines; i < HorizontalLines; i++)
            {
                float Y = i * GridSize + WorldCenter.Y;
                PointF From = new PointF(-HorizontalWidth + WorldCenter.X, Y);
                PointF To = new PointF(HorizontalWidth + WorldCenter.X, Y);
                DrawLine("rgba(30,30,30,.5)",(float).25, this.W2S(From.X, From.Y), this.W2S(To.X, To.Y));
            }

            int VerticalLines = clientWidth / GridSize / 2;
            int VerticalWidth = clientHeight / 2;

            for (int i = -VerticalLines; i < VerticalLines; i++)
            {
                float X = i * GridSize + WorldCenter.X;
                PointF From = new PointF(X, -VerticalWidth + WorldCenter.Y);
                PointF To = new PointF(X, VerticalWidth + WorldCenter.Y);
                DrawLine("rgba(30,30,30,.5)", (float).25, this.W2S(From.X, From.Y), this.W2S(To.X, To.Y));
            }
        }



        public virtual void DrawLine(string color, float lineWidth, PointF ScreenFrom, PointF ScreenTo) {
#if DOTNET
            Pen p = new Pen(Brushes.Blue, lineWidth);
            oGraphics.DrawLine(p, ScreenFrom, ScreenTo);

#else
            this.context.save();

            this.context.lineWidth = lineWidth;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.moveTo(ScreenFrom.X, ScreenFrom.Y);
            this.context.lineTo(ScreenTo.X, ScreenTo.Y);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }



        public void DrawRectangle(string color, float width,float height, float CenterX, float CenterY)
        {
#if DOTNET
            Pen p = new Pen(Brushes.Black, 1);
            float upperleftX = CenterX - width / 2;
            float upperleftY = CenterY - height / 2;
            oGraphics.DrawRectangle(p, upperleftX, upperleftY, width, height);
#else
            this.context.save();

            this.context.lineWidth = 1;
            this.context.beginPath();

            this.context.strokeStyle = color;

            this.context.strokeRect(CenterX - width/2, CenterY - height/2, width, height);

            this.context.closePath();
            this.context.stroke();

            this.context.restore();
#endif
        }



        /// <summary>
        /// Get the upper left world coordinate of the current picture box
        /// </summary>
        /// <returns></returns>
        protected PointF GetUpperLeftWorld()
        {
            // based on the zoom get the world coordinates coordinates of the upper left corner of the screen

            float divisor = 1;

            if (CurrentZoom == .125) divisor = 16;
            if (CurrentZoom == .25) divisor = 8;
            if (CurrentZoom == .5) divisor = 4;
            if (CurrentZoom == 1) divisor = 2;
            if (CurrentZoom == 2) divisor = 1;
            if (CurrentZoom == 4) divisor = (float).5;
            if (CurrentZoom == 8) divisor = (float).25;

            return new PointF(WorldPointScrollX - PictureBoxWidth / divisor, WorldPointScrollY - PictureBoxHeight / divisor);
        }





#if DOTNET
        public Graphics oGraphics { get; set; }
#else
        public CanvasRenderingContext2D context;
#endif

        public Point LastScreenMousePosition { get; set; }
        public eMouseState MouseState = eMouseState.Nothing;

        // This is the bias for display
        public int WorldPointScrollX { get; set; } = 0;
        public int WorldPointScrollY { get; set; } = 0;

        public int PictureBoxWidth { get; set; }
        public int PictureBoxHeight { get; set; }

        public float CurrentZoom { get; set; } = 1;





        // The drawing area may be set up to snap to a grid size.
        public int GridSize { get; set; } = 10;

        public int RoundToGrid(int value)
        {
            int GridSizeHalf = (int)Math.floor((double)GridSize / (double)2);

            int ModX = value % GridSize;
            int DivX = (int)Math.floor((double)value / (double)GridSize);
            int SnapValue = DivX * GridSize;
            if (ModX > GridSizeHalf) SnapValue += GridSize;

            return SnapValue;
        }





        public PointF S2W(float ScreenX, float ScreenY)
        {
            PointF WorldUpperLeft = GetUpperLeftWorld();

            // bias the screen points according to the zoom and add to the upper left corner

            return new PointF(WorldUpperLeft.X + ScreenX * CurrentZoom, WorldUpperLeft.Y + ScreenY * CurrentZoom);

        }

     
        


        /*
         * When the zoom level changes we have to adjust the scroll to compensate.
         * to align the centers of the screen shift the world
         */
        public void SetZoom(float NewZoom)
        {
            CurrentZoom = NewZoom;
        }



      

        


        /*
         * Starting routine to draw shapes. This will be overridden by view objects to perform drawing,
         * and those classes will call methods in this class to do the drawing
         */
        public virtual void DrawShapes()
        {

        }



  
        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
        public virtual eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            return eMouseDownCapture.Nothing;
        }

        public virtual bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            return false;
        }

        public virtual void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {

        }




        public PointF W2S(float WorldX, float WorldY)
        {
            PointF WorldUpperLeft = GetUpperLeftWorld();

            return new PointF((WorldX - WorldUpperLeft.X) / CurrentZoom, (WorldY - WorldUpperLeft.Y) / CurrentZoom);

        }




}
public class Edit2DGraph : Edit2DBase
{
        // Add a new line into the simple layout struct, new vertices and a new edge

        public eOperationStatus AddEdge(PointF WorldFrom, PointF WorldTo,int Width, int Height)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            MostRecentlySelectedLayer.AddEdge(WorldFrom, WorldTo, Width, Height);

#if false
            int nextindex = FindNextIndex();

            Vertex vP1 = new Vertex();
            vP1.Index = nextindex++;
            vP1.X = WorldFrom.X;
            vP1.Y = WorldFrom.Y;
            VertexList.Add(vP1);

            Vertex vP2 = new Vertex();
            vP2.Index = nextindex++;
            vP2.X = WorldTo.X;
            vP2.Y = WorldTo.Y;
            VertexList.Add(vP2);

            // Now add a segment

            Edge oEdge = new Edge();
            oEdge.Height = 30;
            oEdge.Width = 10;
            oEdge.p1 = vP1.Index;
            oEdge.p2 = vP2.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);
#endif
            // redraw shapes
            DrawShapes();
 
            return eOperationStatus.OK;

        }


        public eOperationStatus AddEdgeAtScreenPosition(int OffsetX, int OffsetY, int ScreenLength, int Width, int Height)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            PointF p = new PointF(OffsetX, OffsetY);

            var WorldCenter = this.S2W(p.X,p.Y);
            float Scale = this.CurrentZoom * ScreenLength;

            PointF WorldFrom = new PointF(WorldCenter.X - Scale, WorldCenter.Y - Scale);
            PointF WorldTo = new PointF(WorldCenter.X + Scale, WorldCenter.Y + Scale);

            return AddEdge(WorldFrom, WorldTo, Width, Height);

        }


        public eOperationStatus AddHoleInMostRecentlySelectedEdge(int Width)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.AddHoleInMostRecentlySelectedEdge(Width);

            if (sts != eOperationStatus.OK) return sts;
        
            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;
        }


        public eOperationStatus AppendEdge(int Width, int Height,int DeltaX, int DeltaY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.AppendEdge(Width, Height, DeltaX, DeltaY);
            if (sts != eOperationStatus.OK) return sts;
#if false
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Add a new point
            int nextindex = FindNextIndex();

            Vertex vP = new Vertex();
            vP.Index = nextindex;
            vP.X = MostRecentlySelectedVertex.X + DeltaX;
            vP.Y = MostRecentlySelectedVertex.Y + DeltaY;
            VertexList.Add(vP);

            // Add an edge 

            Edge oEdge = new Edge();
            oEdge.Height = Height;
            oEdge.Width = Width;
            oEdge.p1 = MostRecentlySelectedVertex.Index;
            oEdge.p2 = vP.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);
#endif

            // Redraw shapes
            DrawShapes();

            return eOperationStatus.OK;
        }


        public Edit2DGraph()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif
            // Create the layer list
            Edit2dGraphLayerList = new List<Edit2DGraphLayer>();

            // We would like to just call pushlayer() here, but it calls DrawShapes and that environment isn't set up
            Edit2DGraphLayer oNewLayer = new Edit2DGraphLayer();
            Edit2dGraphLayerList.Add(oNewLayer);
            MostRecentlySelectedLayer = oNewLayer;

            HorizontalScale = 1;
            VerticalScale = 1;

            bShowHandles = true;

        }


        public eOperationStatus DeleteCurrentEdge()
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.DeleteCurrentEdge();
            if (sts != eOperationStatus.OK) return sts;


            // Trigger redraw

            DrawShapes();

            return eOperationStatus.OK;
        }



        public eOperationStatus DeleteSelectedVertex()
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            // If there is not a most recently selected handle return

            eOperationStatus sts = MostRecentlySelectedLayer.DeleteSelectedVertex();

            if (sts != eOperationStatus.OK) return sts;

#if false
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            /*
             * This vertex must have exactly two different edges coming into it
             */
            int ReferenceCount = 0;
            List<Edge> ConnectingEdgeList = new List<Edge>();

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
                if (oEdge.p2 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
            }

            if (ReferenceCount != 2) return eOperationStatus.MustBeTwoConnectingEdgesForOperation;

            // Get the from and to points of the remade edge
            int NewP1 = -1;
            int NewP2 = -1;

            Edge oEdge0 = ConnectingEdgeList.GetFrom(0);
            if (oEdge0.p1 == MostRecentlySelectedVertex.Index)
            {
                NewP1 = oEdge0.p2;
            } else
            {
                NewP1 = oEdge0.p1;
            }

            Edge oEdge1 = ConnectingEdgeList.GetFrom(1);
            if (oEdge1.p2 == MostRecentlySelectedVertex.Index)
            {
                NewP2 = oEdge1.p1;
            }
            else
            {
                NewP2 = oEdge1.p2;
            }

            // Replace the p1 and p2 in the 0th edge and delete the 1th edge. We know that the current handle
            // will be redundant so we can delete it.

            oEdge0.p1 = NewP1;
            oEdge0.p2 = NewP2;

            // delete the current handle, it the one we're removing 
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedVertex.Index)
                {
                    VertexList.RemoveAt(i);
                    break;
                }
            }

            // Delete the 1th edge. 

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge e = EdgeList.GetFrom(i);
                if (e == oEdge1)
                {
                    EdgeList.RemoveAt(i);
                    break;
                }
            }
#endif

            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;

        }


        public void DrawShapes_Layer(Edit2DGraphLayer oLayer,bool IsCurrentLayer, bool bShowHandles)
        {
            // Draw the edges
            for (int i = 0; i < oLayer.EdgeList.Count; i++)
            {
                // Get screen From and To
                Edge oEdge = oLayer.EdgeList.GetFrom(i);

                bool bIsCurrentEdge = oEdge == oLayer.MostRecentlySelectedEdge;

                PointF ScreenFrom = oLayer.GetPointFromIndex(oEdge.p1);
                ScreenFrom = this.W2S(ScreenFrom.X, ScreenFrom.Y);
                PointF ScreenTo = oLayer.GetPointFromIndex(oEdge.p2);
                ScreenTo = this.W2S(ScreenTo.X, ScreenTo.Y);

                // If this is the current layer draw the handles
                if (IsCurrentLayer && bShowHandles)
                {
                    // Get the center point of this edge
                    PointF WorldEdgeCenter = oLayer.EdgeCenter(oEdge);
                    PointF ScreenEdgeCenterFrom = this.W2S(WorldEdgeCenter.X, WorldEdgeCenter.Y);
                    // Get the normal. This points to the 'front'. The front of the panel will 
                    // be where holes are laid out, left to right, while facing it.
                    float dx = ScreenFrom.X - ScreenTo.X;
                    float dy = ScreenFrom.Y - ScreenTo.Y;
                    SVector2 normal = new SVector2(dy, -dx);
                    SVector2 n2 = SVector2.Normalize(normal);
                    SVector2 n3 = SVector2.Scale(n2, 20);

                    // Draw a line at this normal -dy,dx from the center
                    PointF ScreenEdgeCenterTo = new PointF(ScreenEdgeCenterFrom.X + n3.X, ScreenEdgeCenterFrom.Y + n3.Y);
                    this.DrawLine("#ff0000", 1, ScreenEdgeCenterFrom, ScreenEdgeCenterTo);


                    // Draw handles
                    string color = "rgba(255,0,0,.5)";
                    if (oLayer.MostRecentlySelectedVertex != null && oEdge.p1 == oLayer.MostRecentlySelectedVertex.Index)
                    {
                        color = "rgba(0,0,255,1)";
                    }
                    this.DrawCircle(color, 10, ScreenFrom.X, ScreenFrom.Y);

                    color = "rgba(255,0,0,.5)";
                    if (oLayer.MostRecentlySelectedVertex != null && oEdge.p2 == oLayer.MostRecentlySelectedVertex.Index)
                    {
                        color = "rgba(0,0,255,1)";
                    }
                    this.DrawCircle(color, 10, ScreenTo.X, ScreenTo.Y);

                    PointF oCenter = oLayer.EdgeCenter(oEdge);
                    oCenter = this.W2S(oCenter.X, oCenter.Y);

                    // Draw the handle for the edge
                    color = "rgba(0,255,0,.5)";
                    if (bIsCurrentEdge)
                    {
                        color = "rgba(0,0,255,1)";
                    }

                    // Draw the handle for the edge
                    this.DrawRectangle(color, 10, 10, oCenter.X, oCenter.Y);
                }

                // Call draw routine to draw line
                this.DrawLine("#0000FF", (float)0.5, ScreenFrom, ScreenTo);
            }
        }



        public PointF EdgeCenter(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;
            /*
             * The connection point for this edge is the normal
             */
            return new PointF((dx) / 2 + P2.X, (dy) / 2 + P2.Y);
        }


        public double EdgeLength(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;

            return Math.sqrt((dx * dx + dy * dy));

        }


        public Edge FindEdgeWithHoleGroupID(string HoleGroupID)
        {
            for (int i=0; i < Edit2dGraphLayerList.Count; i++)
            {
                Edit2DGraphLayer oLayer = Edit2dGraphLayerList.GetFrom(i);

                for (int j=0; j < oLayer.EdgeList.Count; j++)
                {
                    Edge oEdge = oLayer.EdgeList.GetFrom(j);
                    if (oEdge.HoleGroupID == HoleGroupID) return oEdge;
                }
            }

            return null;
        }
      


        /// <summary>
        /// Find the shape that's closest to the mouse X,Y
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        protected Edge FindEdgeFromMouse (int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return null;

            for (int i=0; i < MostRecentlySelectedLayer.EdgeList.Count; i++)
            {
                Edge pe = MostRecentlySelectedLayer.EdgeList.GetFrom(i);

                // Find the center point of the edge

                PointF Center = MostRecentlySelectedLayer.EdgeCenter(pe);

                PointF CenterScreen = this.W2S(Center.X, Center.Y);

                double distance = Math.sqrt(
                                 (ScreenMouseX - CenterScreen.X) * (ScreenMouseX - CenterScreen.X) +
                                 (ScreenMouseY - CenterScreen.Y) * (ScreenMouseY - CenterScreen.Y));

                if (distance < 10)
                {
                    return pe;
                }
            }

            return null;
        }


        /// <summary>
        /// Find all handles that are close to the current screen position
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        public List<Vertex> FindVertextListAtScreenPoint(int ScreenMouseX,int ScreenMouseY)
        {
            List<Vertex> list = new List<Vertex>();

            if (MostRecentlySelectedLayer == null) return list;

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);               
                PointF ScreenCoordinates = W2S(v.X,v.Y);

                double distance = Math.sqrt(
                     (ScreenMouseX - ScreenCoordinates.X) * (ScreenMouseX - ScreenCoordinates.X) +
                     (ScreenMouseY - ScreenCoordinates.Y) * (ScreenMouseY - ScreenCoordinates.Y));

                if (distance < 10)
                {
                    list.Add(v);
                }
            }

            return list;
        }


        public PointF? FindInsideIntersectionPoint(Edge peGreen, Edge peBlue)
        {
            if (MostRecentlySelectedLayer == null) return null;

            return MostRecentlySelectedLayer.FindInsideIntersectionPoint(peGreen, peBlue);

#if false
            // We can see now if there is a common point. If there is a common point they don't intersect
            if (peGreen.p1 == peBlue.p1 || 
                peGreen.p1 == peBlue.p2 ||
                peGreen.p2 == peBlue.p1 ||
                peGreen.p2 == peBlue.p2)
            {
                // no intersection, there is a common point
                return null;
            }

            Vertex GreenFrom = FindVertexFromIndex(peGreen.p1);
            Vertex GreenTo = FindVertexFromIndex(peGreen.p2);

            Vertex BlueFrom = FindVertexFromIndex(peBlue.p1);
            Vertex BlueTo = FindVertexFromIndex(peBlue.p2);

            /*
             * Are they parallel?
             */

            float par = (float)((GreenTo.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) -
                           (GreenTo.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X));

            if (par == 0)
            {
                return null;                               /* parallel lines */
            }
            /*
             * Find the proportional distance from one point to another
             */
            float tp = ((BlueFrom.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) - (BlueFrom.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X)) / par;
            float tq = ((GreenTo.Y - GreenFrom.Y) * (BlueFrom.X - GreenFrom.X) - (GreenTo.X - GreenFrom.X) * (BlueFrom.Y - GreenFrom.Y)) / par;
            /*
             * If the distance isn't between 0 and 1 the segments don't intersect
             */
            if (tp < 0 || tp > 1 || tq < 0 || tq > 1)
            {
                return null;
            }

            return new PointF(GreenFrom.X + tp * (GreenTo.X - GreenFrom.X), GreenFrom.Y + tp * (GreenTo.Y - GreenFrom.Y));
#endif
        }


        // Flip p1 and p2, effectively changing what direction is considered the 'front' for purposes of placing holes

        public void FlipMostRecentlySelectedEdge()
        {
            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.FlipMostRecentlySelectedEdge();
#if false
            int tmp = MostRecentlySelectedEdge.p1;
            MostRecentlySelectedEdge.p1 = MostRecentlySelectedEdge.p2;
            MostRecentlySelectedEdge.p2 = tmp;
#endif

            DrawShapes();

        }
      


        public eOperationStatus MoveCurrentEdgeByScreenDelta(int ScreenDeltaX, int ScreenDeltaY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            int WorldDeltaX = (int)((float)ScreenDeltaX * this.CurrentZoom);
            int WorldDeltaY = (int)((float)ScreenDeltaY * this.CurrentZoom);

            return MostRecentlySelectedLayer.MoveCurrentEdgeByWorldDelta(WorldDeltaX, WorldDeltaY);

        }


        protected Vertex NewVertex(float X, float Y)
        {
#if false
            Vertex vNew = new Vertex();
            vNew.Index = FindNextIndex();
            vNew.X = X;
            vNew.Y = Y;

            VertexList.Add(vNew);

            return vNew;
#endif
            return new Vertex();
        }
      


        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            /*
              * Try to select a vertex handle first, then an edge handle. The order actually isn't important,
             * only that we try to select one or the other
             */
            if (TryHandleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            // See if we selected an edge handle
            if (TryEdgeSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.EdgeHandle;

            return eMouseDownCapture.Nothing;
        }

   



        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex != null)
            {
                UpdateCurrentHandleToScreenPoint(ScreenMouseX, ScreenMouseY);
                return true;
            }

            if (MostRecentlySelectedLayer.CurrentlySelectedEdge != null)
            {
                MoveCurrentEdgeByScreenDelta(ScreenDeltaX, ScreenDeltaY);
                return true;
            }


            return false;
        }




        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return;

            // If we were dragging a vertex around see if we can merge it with an existing close vertex
            TryMergeVertex(ScreenMouseX, ScreenMouseY);
            /*
             * Reset the current handle and current edge
             */
            MostRecentlySelectedLayer.CurrentlySelectedVertex = null;

            MostRecentlySelectedLayer.CurrentlySelectedEdge = null;
        }




        // ---------------------------------------------------
        public override void DrawShapes()
        {
            this.Clear("#FFFFFF");

            DrawGrid();

            DrawAxis();

            for (int i=0; i < Edit2dGraphLayerList.Count; i++)
            {
                Edit2DGraphLayer olayer = Edit2dGraphLayerList.GetFrom(i);

                DrawShapes_Layer(olayer, olayer == MostRecentlySelectedLayer,bShowHandles);
            }

         }


        public PointF EdgeP1(Edge oEdge)
        {
            if (MostRecentlySelectedLayer == null) return new PointF(0, 0);

            return MostRecentlySelectedLayer.EdgeP1(oEdge);

            //return new PointF(0, 0);

        }

        public PointF EdgeP2(Edge oEdge)
        {
            if (MostRecentlySelectedLayer == null) return new PointF(0, 0);

            return MostRecentlySelectedLayer.EdgeP2(oEdge);

            //return new PointF(0, 0);
        }



        public void PopLayer()
        {
            // Remove the layer at the end of the list. this is like a stack where the 'top' is the end of the list

            if (Edit2dGraphLayerList.Count == 0) return;

            int ndx = Edit2dGraphLayerList.Count - 1;
            Edit2dGraphLayerList.RemoveAt(ndx);

            if (Edit2dGraphLayerList.Count == 0)
            {
                MostRecentlySelectedLayer = null;
            } else {
                MostRecentlySelectedLayer = Edit2dGraphLayerList.GetFrom(Edit2dGraphLayerList.Count - 1);
            }

            DrawShapes();
        }


        public List<Edit2DGraphLayer> Edit2dGraphLayerList { get; set; }
        public Edit2DGraphLayer MostRecentlySelectedLayer { get; set; }

        // These values will scale the drawings as they are rendered. Horizontal in this case means X-Y plane, where Z is Vertical
        public float HorizontalScale { get; set; }
        public float VerticalScale { get; set; }

        // This is global for all layers to allow a preview
        public bool bShowHandles { get; set; }




        public void PushLayer()
        {
            // Insert a new layer at the end of the list. This is like a stack in which the 'top' is the end of the list

            Edit2DGraphLayer oNewLayer = new Edit2DGraphLayer();

            Edit2dGraphLayerList.Add(oNewLayer);

            MostRecentlySelectedLayer = oNewLayer;

            DrawShapes();
        }
      


        public void SelectEdgeByHoleGroupID(string HoleGroupID)
        {
            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.SelectEdgeByHoleGroupID(HoleGroupID);

        }
      


        public void SplitEdgesAtIntersection() {

            if (MostRecentlySelectedLayer == null) return;

            MostRecentlySelectedLayer.SplitEdgesAtIntersection();

#if false
            // Set a tolerence of the thinnest edge. 

            bool ThereAreSplits = true;

            while (ThereAreSplits)
            {
                ThereAreSplits = false;

                for (int i = 0; i < EdgeList.Count && ThereAreSplits == false; i++)
                {
                    Edge S1 = EdgeList.GetFrom(i);
                    
                    for (int j = i + 1; j < EdgeList.Count && ThereAreSplits == false; j++)
                    {
                        Edge S2 = EdgeList.GetFrom(j);
                        PointF? p = FindInsideIntersectionPoint(S1, S2);

                        if (p != null)
                        {
                            // Add this as a new vertex
                            Vertex vNew = new Vertex();
#if DOTNET
                            vNew.X = p.Value.X;
                            vNew.Y = p.Value.Y;
#else
                            vNew.X = p.X;
                            vNew.Y = p.Y;
#endif
                            vNew.Index = FindNextIndex();
                            VertexList.Add(vNew);
                            // TBD make sure its not too close to an existing point

                            // Create two new segments with the intersection as the 'from' point. 

                            Edge pe1 = new Edge();
                            pe1.Width = S1.Width;
                            pe1.Height = S1.Height;
                            pe1.ID = "";                // new segment, remove any id
                            pe1.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe1.p1 = vNew.Index;
                            pe1.p2 = S1.p2;
                            EdgeList.Add(pe1);

                            Edge pe2 = new Edge();
                            pe2.Width = S1.Width;
                            pe2.Height = S1.Height;
                            pe2.ID = "";                // new segment, remove any id
                            pe2.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe2.p1 = vNew.Index;
                            pe2.p2 = S2.p2;
                            EdgeList.Add(pe2);

                            // Truncate the existing segments to the new point

                            S1.p2 = vNew.Index;
                            S2.p2 = vNew.Index;

                            // Indicate that there are splits so that the list can be reprocessed

                            ThereAreSplits = true;

                        }

                    }
                }

            }
#endif

            // Trigger redraw of shapes
            DrawShapes();
        }




        public eOperationStatus SplitMostRecentlySelectedEdge(float RelativeDistance)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.SplitMostRecentlySelectedEdge(RelativeDistance);

            if (sts != eOperationStatus.OK) return sts;

 
            // trigger redraw
            DrawShapes();

            return eOperationStatus.OK;
        }



      


        public bool TryEdgeSelect(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            Edge oEdge = FindEdgeFromMouse(ScreenMouseX, ScreenMouseY);
            if (oEdge != null)
            {
                // Select this edge, this is used for moving the edge while drawing
                MostRecentlySelectedLayer.CurrentlySelectedEdge = oEdge;

                // Remember this as the most recently selected edge, for purposes of commands
                MostRecentlySelectedLayer.MostRecentlySelectedEdge = oEdge;

                return true;
            }

            return false;
        }


        public bool TryHandleSelect(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return false;

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);

                PointF ScreenCoordinates = this.W2S(v.X, v.Y);

                double distance = Math.sqrt(
                    (ScreenMouseX - ScreenCoordinates.X) * (ScreenMouseX - ScreenCoordinates.X) +
                    (ScreenMouseY - ScreenCoordinates.Y) * (ScreenMouseY - ScreenCoordinates.Y));

                if (distance < 10)
                {
                    // For purposes of dragging the handle we want to know the currently selected handle. However, its 
                    // also useful to remember the most recently selected handle for purposes of operations

                    MostRecentlySelectedLayer.CurrentlySelectedVertex = v;
                    MostRecentlySelectedLayer.MostRecentlySelectedVertex = v;
                    return true;
                }
            }

            return false;   // no handle selected
        }


        /// <summary>
        /// If there is a current handle see if we dropped it over another handle, and if so merge these points
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        public eOperationStatus TryMergeVertex(int ScreenMouseX, int ScreenMouseY) 
        {

            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Get all handles that are close to this point. The current handle will be one of them
            // There could conceivable be many handles if they are close. Pick the first handle that 
            // isn't the current handle and merge into that

            // get the first point that is within 10 pixels of the currently selected vertex
            // we want to set the x,y of the current vertex to the x,y of the closest vertex

            List<Vertex> Vertexlist = FindVertextListAtScreenPoint(ScreenMouseX, ScreenMouseY);

            // Pick the first one that isn't the currently selected edge

            Vertex MergeToVertex = null;
            for (int i=0; i < Vertexlist.Count; i++)
            {
                Vertex v = Vertexlist.GetFrom(i);
                if (v == MostRecentlySelectedLayer.CurrentlySelectedVertex) continue;

                MergeToVertex = v;
                break;

            }

            // If there were no other vertices close we're done
            if (MergeToVertex == null) return eOperationStatus.OK;

            /*
             * Replace all instances of the currently selected vertex with the merged vertex in existing edges
             */
            for (int i=0; i < MostRecentlySelectedLayer.EdgeList.Count; i++)
            {
                Edge oEdge = MostRecentlySelectedLayer. EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    oEdge.p1 = MergeToVertex.Index;
                }

                if (oEdge.p2 == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    oEdge.p2 = MergeToVertex.Index;
                }
            }

            // delete the currently selected vertex

            for (int i=0; i < MostRecentlySelectedLayer.VertexList.Count; i++)
            {
                Vertex v = MostRecentlySelectedLayer.VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedLayer.CurrentlySelectedVertex.Index)
                {
                    MostRecentlySelectedLayer.VertexList.RemoveAt(i);
                    break;
                }
            }

            // Set the most recently selected vertex to the merge vertex

            MostRecentlySelectedLayer.MostRecentlySelectedVertex = MergeToVertex;

            return eOperationStatus.OK;

        }   



        public eOperationStatus UnMergeMostRecentlySelectedHandle()
        {

            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            eOperationStatus sts = MostRecentlySelectedLayer.UnMergeMostRecentlySelectedHandle();

            if (sts != eOperationStatus.OK) return sts;

            // trigger a redraw
            DrawShapes();

            return eOperationStatus.OK;

        }
           



        public eOperationStatus UpdateCurrentHandleToScreenPoint(int ScreenMouseX, int ScreenMouseY)
        {
            if (MostRecentlySelectedLayer == null) return eOperationStatus.NoLayerSelected;

            if (MostRecentlySelectedLayer.CurrentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if ( this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            MostRecentlySelectedLayer.CurrentlySelectedVertex.X = (float) newx;
            MostRecentlySelectedLayer.CurrentlySelectedVertex.Y = (float) newy;    

            return eOperationStatus.OK;
        }



}
public class Edit2DGraphLayer
{
        // Add a new line into the simple layout struct, new vertices and a new edge

        public void AddEdge(PointF WorldFrom, PointF WorldTo,int Width, int Height)
        {
            int nextindex = FindNextIndex();

            Vertex vP1 = new Vertex();
            vP1.Index = nextindex++;
            vP1.X = WorldFrom.X;
            vP1.Y = WorldFrom.Y;
            VertexList.Add(vP1);

            Vertex vP2 = new Vertex();
            vP2.Index = nextindex++;
            vP2.X = WorldTo.X;
            vP2.Y = WorldTo.Y;
            VertexList.Add(vP2);

            // Now add a segment

            Edge oEdge = new Edge();
            oEdge.Height = 30;
            oEdge.Width = 10;
            oEdge.p1 = vP1.Index;
            oEdge.p2 = vP2.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);

        }


        public eOperationStatus AddHoleInMostRecentlySelectedEdge(int Width)
        {

            if (EdgeList.Count == 0)
            {
                return eOperationStatus.NoEdgesDefined;
            }

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }

            double thisEdgeLength = EdgeLength(MostRecentlySelectedEdge);

            if (Width >= thisEdgeLength) return eOperationStatus.EdgeNotWideEnoughForOperation;

            // Determine the interpolations based on the width and length

            float WidthPercent = (float)(Width / thisEdgeLength);

            Vertex v1 = FindVertexFromIndex(MostRecentlySelectedEdge.p1);
            Vertex v2 = FindVertexFromIndex(MostRecentlySelectedEdge.p2);
            /*
             * Create vectors of the end points
             */
            SVector2 f = new SVector2(v1.X, v1.Y);
            SVector2 t = new SVector2(v2.X, v2.Y);

            // Find a new end point and add a vertex for it
            float lowpercent = (float) .5 - WidthPercent;
            SVector2 LowPartVector = SVector2.Interpolate(f, t, lowpercent);
            Vertex vNew1 = NewVertex(LowPartVector.X, LowPartVector.Y);

            // Create a new edge and add it
            Edge newEdge = new Edge();
            newEdge.Width = MostRecentlySelectedEdge.Width;
            newEdge.Height = MostRecentlySelectedEdge.Height;
            newEdge.ID = "";
            newEdge.HoleGroupID = "";
            newEdge.p1 = MostRecentlySelectedEdge.p1;
            newEdge.p2 = vNew1.Index;
            EdgeList.Add(newEdge);

            // Add a new vertex for the other position on the other side of the hole
            float highpercent =  (float) .5 + WidthPercent;
            SVector2 HighPartVector = SVector2.Interpolate(f, t, highpercent);
            Vertex vNew2 = NewVertex(HighPartVector.X, HighPartVector.Y);

            // Modify the currently selected edge so that it starts at the vnew2 

            MostRecentlySelectedEdge.p1 = vNew2.Index;

            return eOperationStatus.OK;
        }


        public eOperationStatus AppendEdge(int Width, int Height,int DeltaX, int DeltaY)
        {
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Add a new point
            int nextindex = FindNextIndex();

            Vertex vP = new Vertex();
            vP.Index = nextindex;
            vP.X = MostRecentlySelectedVertex.X + DeltaX;
            vP.Y = MostRecentlySelectedVertex.Y + DeltaY;
            VertexList.Add(vP);

            // Add an edge 

            Edge oEdge = new Edge();
            oEdge.Height = Height;
            oEdge.Width = Width;
            oEdge.p1 = MostRecentlySelectedVertex.Index;
            oEdge.p2 = vP.Index;
            oEdge.ID = "";
            oEdge.HoleGroupID = "";

            EdgeList.Add(oEdge);

            return eOperationStatus.OK;
        }


        public void AppendFromJSON(Edge[] EdgeArray, Vertex[] VertexArray)
        {
            // We need to adjust the vertex index values so they don't overlap with the current vertices
            int NewBase = 0;
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index > NewBase)
                {
                    NewBase = v.Index;
                }
            }

            // Set new base. Add this value to the vertex and edge vertex indeces
            NewBase++;

            /*
             * Add the new points and vertices to this layer, adjusting point indices
             */
            for (int i=0; i < VertexArray.Length; i++)
            {
                // Use 'copyfrom' because the vertexarray[] isn't a full object, its only properties in JSON
                Vertex v = Vertex.CopyFrom(VertexArray[i]);
                v.Index += NewBase;

                VertexList.Add(v);
            }

            for (int i = 0; i < EdgeArray.Length; i++)
            {
                // Use 'copyfrom' because the EdgeArray[] isn't a full object, its only properties in JSON
                Edge e = Edge.CopyFrom(EdgeArray[i]);
                e.p1 += NewBase;
                e.p2 += NewBase;
                e.HoleGroupID = "";
                e.ID = "";

                EdgeList.Add(e);
            }
        }


        public Edit2DGraphLayer()
        {
            VertexList = new List<Vertex>();
            EdgeList = new List<Edge>();
        }


        public eOperationStatus DeleteCurrentEdge()
        {
            // delete the most recently selected edge, 
            Edge oEdgeToDelete = null;

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }
            int IndexOfEdgeToDelete = -1;
            for (int i=0; i < EdgeList.Count; i++)
            {
                oEdgeToDelete = EdgeList.GetFrom(i);

                if (oEdgeToDelete == MostRecentlySelectedEdge) { 
                    IndexOfEdgeToDelete = i;
                    break;
                }
            }

            if (IndexOfEdgeToDelete == -1) return eOperationStatus.NoEdgeSelected;
            
            /*
             * When deleting an edge we have to account for the vertices. Each of the vertices may be shared or not. 
             * If a vertex isn't shared we delete it as well
             */
            int p1ReferenceCount = 0;
            int p2ReferenceCount = 0;

            for (int i = 0; i < EdgeList.Count; i++)
            {
                Edge e = EdgeList.GetFrom(i);

                // Note that this will include the edge we are deleting. that's fine, we account for that

                if (e.p1 == oEdgeToDelete.p1) p1ReferenceCount++;
                if (e.p2 == oEdgeToDelete.p1) p1ReferenceCount++;

                if (e.p1 == oEdgeToDelete.p2) p2ReferenceCount++;
                if (e.p2 == oEdgeToDelete.p2) p2ReferenceCount++;
            }
            /*
             * If either of the reference counts are 1 that means that only the edge to be deleted contained it, 
             * and we can delete that vertex
             */
             if (p1ReferenceCount == 1)
            {
                for (int i=0; i < VertexList.Count; i++)
                {
                    Vertex v = VertexList.GetFrom(i);
                    if (v.Index == oEdgeToDelete.p1)
                    {
                        VertexList.RemoveAt(i);
                        break;
                    }
                }
            }

            if (p2ReferenceCount == 1)
            {
                for (int i = 0; i < VertexList.Count; i++)
                {
                    Vertex v = VertexList.GetFrom(i);
                    if (v.Index == oEdgeToDelete.p2)
                    {
                        VertexList.RemoveAt(i);
                        break;
                    }
                }
            }

            // Now delete the edge

            EdgeList.RemoveAt(IndexOfEdgeToDelete);

            // clear the most recently selected edge and vertex, in case it was on this edge
            MostRecentlySelectedEdge = null;
            MostRecentlySelectedVertex = null;
            CurrentlySelectedEdge = null;
            CurrentlySelectedVertex = null;

            return eOperationStatus.OK;

        }



        public eOperationStatus DeleteSelectedVertex()
        {

            // If there is not a most recently selected handle return

            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            /*
             * This vertex must have exactly two different edges coming into it
             */
            int ReferenceCount = 0;
            List<Edge> ConnectingEdgeList = new List<Edge>();

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
                if (oEdge.p2 == MostRecentlySelectedVertex.Index)
                {
                    ReferenceCount++;
                    ConnectingEdgeList.Add(oEdge);
                }
            }

            if (ReferenceCount != 2) return eOperationStatus.MustBeTwoConnectingEdgesForOperation;

            // Get the from and to points of the remade edge
            int NewP1 = -1;
            int NewP2 = -1;

            Edge oEdge0 = ConnectingEdgeList.GetFrom(0);
            if (oEdge0.p1 == MostRecentlySelectedVertex.Index)
            {
                NewP1 = oEdge0.p2;
            } else
            {
                NewP1 = oEdge0.p1;
            }

            Edge oEdge1 = ConnectingEdgeList.GetFrom(1);
            if (oEdge1.p2 == MostRecentlySelectedVertex.Index)
            {
                NewP2 = oEdge1.p1;
            }
            else
            {
                NewP2 = oEdge1.p2;
            }

            // Replace the p1 and p2 in the 0th edge and delete the 1th edge. We know that the current handle
            // will be redundant so we can delete it.

            oEdge0.p1 = NewP1;
            oEdge0.p2 = NewP2;

            // delete the current handle, it the one we're removing 
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedVertex.Index)
                {
                    VertexList.RemoveAt(i);
                    break;
                }
            }

            // Delete the 1th edge. 

            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge e = EdgeList.GetFrom(i);
                if (e == oEdge1)
                {
                    EdgeList.RemoveAt(i);
                    break;
                }
            }

            return eOperationStatus.OK;

        }


        public PointF EdgeCenter(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;
            /*
             * The connection point for this edge is the normal
             */
            return new PointF((dx) / 2 + P2.X, (dy) / 2 + P2.Y);
        }


        public double EdgeLength(Edge oEdge)
        {
            PointF P1 = EdgeP1(oEdge);
            PointF P2 = EdgeP2(oEdge);

            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;

            return Math.sqrt((dx * dx + dy * dy));

        }


        public PointF? FindInsideIntersectionPoint(Edge peGreen, Edge peBlue)
        {
            // We can see now if there is a common point. If there is a common point they don't intersect
            if (peGreen.p1 == peBlue.p1 || 
                peGreen.p1 == peBlue.p2 ||
                peGreen.p2 == peBlue.p1 ||
                peGreen.p2 == peBlue.p2)
            {
                // no intersection, there is a common point
                return null;
            }

            Vertex GreenFrom = FindVertexFromIndex(peGreen.p1);
            Vertex GreenTo = FindVertexFromIndex(peGreen.p2);

            Vertex BlueFrom = FindVertexFromIndex(peBlue.p1);
            Vertex BlueTo = FindVertexFromIndex(peBlue.p2);

            /*
             * Are they parallel?
             */

            float par = (float)((GreenTo.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) -
                           (GreenTo.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X));

            if (par == 0)
            {
                return null;                               /* parallel lines */
            }
            /*
             * Find the proportional distance from one point to another
             */
            float tp = ((BlueFrom.X - GreenFrom.X) * (BlueTo.Y - BlueFrom.Y) - (BlueFrom.Y - GreenFrom.Y) * (BlueTo.X - BlueFrom.X)) / par;
            float tq = ((GreenTo.Y - GreenFrom.Y) * (BlueFrom.X - GreenFrom.X) - (GreenTo.X - GreenFrom.X) * (BlueFrom.Y - GreenFrom.Y)) / par;
            /*
             * If the distance isn't between 0 and 1 the segments don't intersect
             */
            if (tp < 0 || tp > 1 || tq < 0 || tq > 1)
            {
                return null;
            }

            return new PointF(GreenFrom.X + tp * (GreenTo.X - GreenFrom.X), GreenFrom.Y + tp * (GreenTo.Y - GreenFrom.Y));

        }


        protected int FindNextIndex()
        {
            // add two new vertices. Find the next available index
            int highestindex = -1;
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex vTmp = VertexList.GetFrom(i);
                if (vTmp.Index > highestindex)
                {
                    highestindex = vTmp.Index;
                }
            }
            int nextindex = highestindex + 1;
            return nextindex;

        } 


        public Vertex FindVertexFromIndex(int index)
        {
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == index) return v;
            }

            return null;
        }


        // Flip p1 and p2, effectively changing what direction is considered the 'front' for purposes of placing holes

        public void FlipMostRecentlySelectedEdge()
        {
            if (MostRecentlySelectedEdge == null) return;

            int tmp = MostRecentlySelectedEdge.p1;
            MostRecentlySelectedEdge.p1 = MostRecentlySelectedEdge.p2;
            MostRecentlySelectedEdge.p2 = tmp;

        }
      


        public PointF GetPointFromIndex(int index)
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == index)
                {
                    return new PointF(v.X, v.Y);
                }
            }
            return new PointF(0, 0);
        }



        public void LoadFromJSON(Edge[] EdgeArray, Vertex[] VertexArray)
        {
            
            VertexList = new List<Vertex>();
            for (int i=0; i < VertexArray.Length; i++)
            {
                VertexList.Add(VertexArray[i]);
            }

            EdgeList = new List<Edge>();
            for (int i = 0; i < EdgeArray.Length; i++)
            {
                EdgeList.Add(EdgeArray[i]);
            }
        }


        public eOperationStatus MoveCurrentEdgeByWorldDelta(int WorldDeltaX, int WorldDeltaY)
        {
            if (CurrentlySelectedEdge == null) return eOperationStatus.NoEdgeSelected;
            /*
             * update the vertices associated with this edge. This will automatically move all edges associated with it
             */

            Vertex P1Vertex = FindVertexFromIndex(CurrentlySelectedEdge.p1);
            P1Vertex.X += WorldDeltaX;
            P1Vertex.Y += WorldDeltaY;


            Vertex P2Vertex = FindVertexFromIndex(CurrentlySelectedEdge.p2);
            P2Vertex.X += WorldDeltaX;
            P2Vertex.Y += WorldDeltaY;

            return eOperationStatus.OK;
        }


        protected Vertex NewVertex(float X, float Y)
        {
            Vertex vNew = new Vertex();
            vNew.Index = FindNextIndex();
            vNew.X = X;
            vNew.Y = Y;

            VertexList.Add(vNew);

            return vNew;
        }
      


        public PointF EdgeP1(Edge oEdge)
        {
            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == oEdge.p1)
                {
                    return new PointF(v.X, v.Y);
                }
            }

            return new PointF(0, 0);
        }

        public PointF EdgeP2(Edge oEdge)
        {
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == oEdge.p2)
                {
                    return new PointF(v.X, v.Y);
                }
            }

            return new PointF(0, 0);
        }



        public List<Vertex> VertexList { get; set; }
        public List<Edge> EdgeList { get; set; }

        // While a handle is being moved this is the vertex associated with it
        public Vertex CurrentlySelectedVertex { get; set; } = null;

        // When a handle is selected we remember the most recent selection so that commands can be performed on it
        public Vertex MostRecentlySelectedVertex { get; set; } = null;

        // While an edge is being moved this is the index of that handle.
        public Edge CurrentlySelectedEdge = null;

        // When an edge is selected we remember the most recent selection so that commands can be performed on it
        public Edge MostRecentlySelectedEdge = null;


        public void SelectEdgeByHoleGroupID(string HoleGroupID)
        {
            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.HoleGroupID == HoleGroupID)
                {
                    MostRecentlySelectedEdge = oEdge;
                    return;
                }
            }
        }
      


        public void SplitEdgesAtIntersection() {

            // Set a tolerence of the thinnest edge. 

            bool ThereAreSplits = true;

            while (ThereAreSplits)
            {
                ThereAreSplits = false;

                for (int i = 0; i < EdgeList.Count && ThereAreSplits == false; i++)
                {
                    Edge S1 = EdgeList.GetFrom(i);
                    
                    for (int j = i + 1; j < EdgeList.Count && ThereAreSplits == false; j++)
                    {
                        Edge S2 = EdgeList.GetFrom(j);
                        PointF? p = FindInsideIntersectionPoint(S1, S2);

                        if (p != null)
                        {
                            // Add this as a new vertex
                            Vertex vNew = new Vertex();
#if DOTNET
                            vNew.X = p.Value.X;
                            vNew.Y = p.Value.Y;
#else
                            vNew.X = p.X;
                            vNew.Y = p.Y;
#endif
                            vNew.Index = FindNextIndex();
                            VertexList.Add(vNew);
                            // TBD make sure its not too close to an existing point

                            // Create two new segments with the intersection as the 'from' point. 

                            Edge pe1 = new Edge();
                            pe1.Width = S1.Width;
                            pe1.Height = S1.Height;
                            pe1.ID = "";                // new segment, remove any id
                            pe1.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe1.p1 = vNew.Index;
                            pe1.p2 = S1.p2;
                            EdgeList.Add(pe1);

                            Edge pe2 = new Edge();
                            pe2.Width = S1.Width;
                            pe2.Height = S1.Height;
                            pe2.ID = "";                // new segment, remove any id
                            pe2.HoleGroupID = "";       // new segment don't try to bring holes over
                            pe2.p1 = vNew.Index;
                            pe2.p2 = S2.p2;
                            EdgeList.Add(pe2);

                            // Truncate the existing segments to the new point

                            S1.p2 = vNew.Index;
                            S2.p2 = vNew.Index;

                            // Indicate that there are splits so that the list can be reprocessed

                            ThereAreSplits = true;

                        }

                    }
                }

            }

        }




        public eOperationStatus SplitMostRecentlySelectedEdge(float RelativeDistance)
        {
            if (EdgeList.Count == 0)
            {
                return eOperationStatus.NoEdgesDefined;
            }

            if (MostRecentlySelectedEdge == null)
            {
                return eOperationStatus.NoEdgeSelected;
            }

            Vertex v1 = FindVertexFromIndex(MostRecentlySelectedEdge.p1);
            Vertex v2 = FindVertexFromIndex(MostRecentlySelectedEdge.p2);

            SVector2 f = new SVector2(v1.X, v1.Y); 
            SVector2 t = new SVector2(v2.X, v2.Y);

            SVector2 interpolatedVector = SVector2.Interpolate(f, t, RelativeDistance);
            /*
             * Get the point at the at this distance mark
             */
            PointF InterpolatedPoint = new PointF(interpolatedVector.X, interpolatedVector.Y);

            // Add the interpolated point as a new vertex
            Vertex vInterpolated = new Vertex();
            vInterpolated.Index = FindNextIndex();
            vInterpolated.X = InterpolatedPoint.X;
            vInterpolated.Y = InterpolatedPoint.Y;

            VertexList.Add(vInterpolated);
            
            // Create a new edge with two points, the interpolated point and the current p2

            Edge newEdge = new Edge();
            newEdge.Width = MostRecentlySelectedEdge.Width;
            newEdge.Height = MostRecentlySelectedEdge.Height;
            newEdge.ID = "";
            newEdge.HoleGroupID = "";
            newEdge.p1 = vInterpolated.Index;
            newEdge.p2 = MostRecentlySelectedEdge.p2;

            EdgeList.Add(newEdge);

            // update the current edge, set its 'p2' point to the interpolated point. This will trim it
            // Also, clear out any holes, the split edge requires a reset

            MostRecentlySelectedEdge.p2 = vInterpolated.Index;
            MostRecentlySelectedEdge.HoleGroupID = "";

            return eOperationStatus.OK;
        }



      



        public eOperationStatus UnMergeMostRecentlySelectedHandle()
        {
            if (MostRecentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            // Delete this index. We'll replace all occurrences with new vertices

            for (int i=0; i < VertexList.Count; i++)
            {
                Vertex v = VertexList.GetFrom(i);
                if (v.Index == MostRecentlySelectedVertex.Index)
                {
                    VertexList.RemoveAt(i);
                    break;
                }
            }
            /*
             * Now create new handles for each instance of the just deleted vertex
             */
            for (int i=0; i < EdgeList.Count; i++)
            {
                Edge oEdge = EdgeList.GetFrom(i);
                if (oEdge.p1 == MostRecentlySelectedVertex.Index)
                {
                    Vertex v = NewVertex(MostRecentlySelectedVertex.X, MostRecentlySelectedVertex.Y);
                    oEdge.p1 = v.Index;
                }
                if (oEdge.p2 == MostRecentlySelectedVertex.Index)
                {
                    Vertex v = NewVertex(MostRecentlySelectedVertex.X, MostRecentlySelectedVertex.Y);
                    oEdge.p2 = v.Index;
                }

            }

            return eOperationStatus.OK;

        }
           


}
public class Edit2DHoleGroup :Edit2DBase
{
        public void AddEllipse(int ScreenOffsetX, int ScreenOffsetY, int Width, int Height)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);

            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "ell";
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;
            /*
             * Save this as the current and most recently selected
             * The most recently selected is used for commands after the mouse is up
             */
            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;
            /*
             * The index is the at the end of the current list. This means we will have to manage the index values
             * when they are deleted
             */          
            oHole.HoleTypeIndex = BoundaryEllipseList.Count;
            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);
            /*
             * Now create and add the ellipse
             */
            BoundaryEllipse ell = new BoundaryEllipse();
            ell.Width = Width;
            ell.Height = Height;

            BoundaryEllipseList.Add(ell);

            // trigger redraw

            DrawShapes();
        }

        public void AddHoleToHoleList(HoleGroup hg, Hole oNewHole)
        {

        }




        public void AddHoleGroup(string HoleGroupID)
        {
            HoleGroup hg = new HoleGroup();
            hg.HoleGroupID = HoleGroupID;
            hg.HoleList = new LayoutHole[0];

            HoleGroupList.Add(hg);
        }


        public void AddHoleToHoleGroup(HoleGroup hg, LayoutHole oNewHole)
        {

#if DOTNET
            List<LayoutHole> tmp = new List<LayoutHole>(hg.HoleList);
            tmp.Add(oNewHole);
            hg.HoleList = tmp.ToArray();
#else
            hg.HoleList.push(oNewHole);
#endif
        }



        // Add a polygon. This is managed with a simplelayout object 
        public void AddPolygon(int ScreenOffsetX, int ScreenOffsetY, int InitialSides, int Radius)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            BoundaryPolygon oPolygon = new BoundaryPolygon();

            oPolygon.PointList = new Point3D[InitialSides];

            float Angle = 0;
            float AngleDelta = (float) Math.PI * 2 / InitialSides;
            for (int i=0; i < InitialSides; i++)
            {
                Point3D p = new Point3D();
#if DOTNET
                float x = (float)Math.Cos(Angle) * Radius;
                float y = (float)Math.Sin(Angle) * Radius;
#else
                float x = (float) Math.cos(Angle) * Radius;
                float y = (float) Math.sin(Angle) * Radius;
#endif
                p.X = x;
                p.Y = y;

                oPolygon.PointList[i] = p;

                Angle += AngleDelta;
            }

            /*
             * Create a hole and boundary polygon object
             */
            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);
            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "poly";
            oHole.HoleTypeIndex = BoundaryPolygonList.Count;
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;

            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;

            BoundaryPolygonList.Add(oPolygon);

            // trigger redraw

            DrawShapes();

        }


        public void AddRectangle(int ScreenOffsetX, int ScreenOffsetY, int Width, int Height)
        {
            if (MostRecentlySelectedHoleGroup == null) return;

            PointF WorldOffset = this.S2W(ScreenOffsetX, ScreenOffsetY);

            LayoutHole oHole = new LayoutHole();
            oHole.HoleType = "rect";
            oHole.OffsetX = WorldOffset.X;
            oHole.OffsetY = WorldOffset.Y;
            /*
             * Save this as the current and most recently selected
             */
            CurrentlySelectedHole = oHole;
            MostRecentlySelectedHole = oHole;
            /*
             * The index is the at the end of the current list. This means we will have to manage the index values
             * when they are deleted
             */          
            oHole.HoleTypeIndex = BoundaryRectangleList.Count;

            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            /*
             * Now create and add the rectangle
             */
            BoundaryRectangle rect = new BoundaryRectangle();
            rect.Width = Width;
            rect.Height = Height;

            BoundaryRectangleList.Add(rect);

            // trigger draw shapes
            DrawShapes();
        }




        public Edit2DHoleGroup()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif
            // This will manage one set of holes; one holegroup

            MostRecentlySelectedHoleGroup = null; 

            MostRecentlySelectedHole = null;

            MostRecentlySelectedPolygonVertexIndex = -1;
            MostRecentlySelectedPolygonEdgeIndex = -1;

            // These are the individual drawing patterns

            BoundaryRectangleList = new List<BoundaryRectangle>();
            BoundaryEllipseList = new List<BoundaryEllipse>();
            BoundaryPolygonList = new List<BoundaryPolygon>();

            // Initialize the list of hole groups
            HoleGroupList = new List<HoleGroup>();

            // Initially set no shaded area
            ShadeLength = -1;
            ShadeHeight = -1; 

        }


        public void DeleteCurrentHole()
        {
            if (MostRecentlySelectedHole == null) return;

            int IndexToDelete = -1;

            switch (MostRecentlySelectedHole.HoleType)
            {
                case "ell":
                    // Delete this index
                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;

                    for (int i = 0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "ell" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryEllipseList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
                case "rect":
                    // Delete this index
                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;
                    for (int i=0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "rect" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryRectangleList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
                case "poly":

                    // Find all references to hole values greater than this one and reduce them by one so the indices are correct
                    IndexToDelete = -1;

                    for (int i = 0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
                    {
                        LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];

                        if (MostRecentlySelectedHole == oHole) IndexToDelete = i;

                        if (oHole.HoleType == "poly" && oHole.HoleTypeIndex > MostRecentlySelectedHole.HoleTypeIndex)
                        {
                            oHole.HoleTypeIndex--;
                        }
                    }

                    // Delete this hole
                    RemoveHoleFromHoleGroup(MostRecentlySelectedHoleGroup, IndexToDelete);

                    // Delete this shape
                    BoundaryPolygonList.RemoveAt(MostRecentlySelectedHole.HoleTypeIndex);
                    break;
            }

            // trigger a repaint
            DrawShapes();
        }




        void DeleteHoleGroup(string HoleGroupID)
        {
            for (int i=0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i);
                if (hg.HoleGroupID == HoleGroupID)
                {
                    HoleGroupList.RemoveAt(i);
                    MostRecentlySelectedHole = null;
                    MostRecentlySelectedHoleGroup = null;
                    MostRecentlySelectedPolygonEdgeIndex = -1;
                    MostRecentlySelectedPolygonVertexIndex = -1;
                    return;
                }
            }
        }


        public void DuplicateCurrentHole()
        {
            if (MostRecentlySelectedHole == null) return;

            LayoutHole oHole = LayoutHole.CopyFrom(MostRecentlySelectedHole);

            // Add at an offset so it will be easy to see
            oHole.OffsetX += 10;
            oHole.OffsetY += 10;

            // Create a new version of the instance of the hole

            int ndx = MostRecentlySelectedHole.HoleTypeIndex;
            switch (MostRecentlySelectedHole.HoleType)
            {
                case "ell":
                    oHole.HoleTypeIndex = BoundaryEllipseList.Count;
                    BoundaryEllipse e = BoundaryEllipseList.GetFrom(ndx);
                    BoundaryEllipseList.Add(BoundaryEllipse.CopyFrom(e));
                    break;
                case "rect":
                    oHole.HoleTypeIndex = BoundaryRectangleList.Count;
                    BoundaryRectangle r = BoundaryRectangleList.GetFrom(ndx);
                    BoundaryRectangleList.Add(BoundaryRectangle.CopyFrom(r));
                    break;
                case "poly":
                    oHole.HoleTypeIndex = BoundaryPolygonList.Count;
                    BoundaryPolygon p = BoundaryPolygonList.GetFrom(ndx);
                    BoundaryPolygonList.Add(BoundaryPolygon.CopyFrom(p));
                    break;
            }

            // Add the new hole to the hole group
            AddHoleToHoleGroup(MostRecentlySelectedHoleGroup, oHole);

            // trigger a repaint
            DrawShapes();
        }



        public PointF EdgeCenter(PointF From, PointF To)
        {
            float dx = From.X - To.X;
            float dy = From.Y - To.Y;
            /*
             * The connection point for this edge is the normal
             */
            return new PointF((dx) / 2 + To.X, (dy) / 2 + To.Y);
        }


        // We have this in a separate routine to make it easier to separate out the JS/C# functionality
        public int GetHoleListLength(LayoutHole[] HoleList)
        {
#if DOTNET
            return HoleList.Length;
#else
            return HoleList.length;
#endif

        }



        /*
         * This routine is used to invert the hole points, specifically the 'Y' points.
         * The html canvas has 0,0 at the upper left and 'Y' goes down. For the holegroup canvas 
         * we want a 'Y' that goes up, because that's how the holes are overlaid and appear in 
         * three dimensions, and how we think of them intuitively. To manage this we'll simply flip the 
         * Y points for the duration of the time that they are being edited and reflip them when its time to save
         */
        public void InvertHolePoints()
        {
            for (int i = 0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i); 

                for (int j = 0; j < hg.HoleList.Length; j++)
                {
                    /*
                     * For all hole types the offset Y is inverted
                     */
                    LayoutHole oHole = hg.HoleList[j];
                    oHole.OffsetY = -oHole.OffsetY;
                    /*
                     * For polygon hole types the individual Y's are inverted
                     */
                    if (oHole.HoleType == "poly")
                    {
                        BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);
                        for ( int k=0; k < oPolygon.PointList.Length; k++)
                        {
                            Point3D p = oPolygon.PointList[k];
                            p.Y = -p.Y;
                        }
                    }
                }
            }
        }



        public void LoadFromJSON(
            HoleGroup[] HoleGroupArray,
            BoundaryRectangle[] RectangleArray,
            BoundaryEllipse[] EllipseArray, 
            BoundaryPolygon[] PolygonArray)
        {

            HoleGroupList = new List<HoleGroup>();
            for (int i=0; i < HoleGroupArray.Length; i++)
            {
                HoleGroup hg = new HoleGroup();

                hg.HoleGroupID = HoleGroupArray[i].HoleGroupID;
                /*
                 * When the objects get here in JS the holelist is no longer a list type, its an array type because
                 * of serialization. 
                 */
                int len = GetHoleListLength(HoleGroupArray[i].HoleList);
                for (int j = 0; j < len; j++)
                {
                    LayoutHole oHoleToAdd = HoleGroupArray[i].HoleList[j];

                    AddHoleToHoleGroup(hg, oHoleToAdd);
                }

                HoleGroupList.Add(hg);
            }

            BoundaryRectangleList = new List<BoundaryRectangle>();
            for (int i=0; i < RectangleArray.Length; i++)
            {
                BoundaryRectangleList.Add(RectangleArray[i]);
            }

            BoundaryEllipseList = new List<BoundaryEllipse>();
            for (int i = 0; i < EllipseArray.Length; i++)
            {
                BoundaryEllipseList.Add(EllipseArray[i]);
            }

            BoundaryPolygonList = new List<BoundaryPolygon>();
            for (int i = 0; i < PolygonArray.Length; i++)
            {
                BoundaryPolygonList.Add(PolygonArray[i]);
            }
            /*
             * Now flip all of the Y points so that positive Y values go 'up'
             */
            InvertHolePoints();
        }




        // ---------------------------------------------------
        public override void DrawShapes()
        {
            this.Clear("#FFFFFF");
            bool bIsCurrentHole = false;

            DrawGrid();

            DrawAxis();

            /*
             * If specified draw in a shaded area that represents the current edge having holes designed for it
             */
            if (ShadeHeight > 0 && ShadeLength > 0)
            {
                PointF LowerLeft  = this.W2S(0, 0);
                PointF LowerRight = this.W2S(ShadeLength, 0);
                PointF UpperRight = this.W2S(ShadeLength, - ShadeHeight);
                PointF UpperLeft  = this.W2S(0, -ShadeHeight);

                this.DrawLine("rgba(100,100,150,.7", 1, LowerLeft, LowerRight);
                this.DrawLine("rgba(100,100,150,.7", 1, LowerRight, UpperRight);
                this.DrawLine("rgba(100,100,150,.7", 1, UpperRight, UpperLeft);
                this.DrawLine("rgba(100,100,150,.7", 1, UpperLeft, LowerLeft);
            }

            if (this.MostRecentlySelectedHoleGroup == null) return;

#if DOTNET
            int len = MostRecentlySelectedHoleGroup.HoleList.Length;
#else
            int len = MostRecentlySelectedHoleGroup.HoleList.length;
#endif

            for (int i=0; i < len; i++)
            {
                LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];
                PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

                bIsCurrentHole = oHole == MostRecentlySelectedHole;

                switch (oHole.HoleType)
                {
                    case "rect":

                        DrawShapes_Rectangle(oHole, bIsCurrentHole);
                        break;

                    case "poly":

                        DrawShapes_Polygon(oHole, bIsCurrentHole);
                        break;

                    case "ell":

                        DrawShapes_Ellipse(oHole, bIsCurrentHole);
                        break;
                }
            }
        }
   

  





  
        //-------------------------------------------------------
        private void DrawShapes_Ellipse(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryEllipse oEllipse = BoundaryEllipseList.GetFrom(oHole.HoleTypeIndex);

            PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

            float SWidth = oEllipse.Width / CurrentZoom;
            float SHeight = oEllipse.Height / CurrentZoom;

            string color = IsCurrentHole ? "#0000FF" : "rgba(255,0,0,.5)";
            // Draw the move handle
            this.DrawCircle(color, 10, Screen.X, Screen.Y);

            // draw the actual ellipse
            this.DrawEllipse("#0000FF", SWidth, SHeight, Screen.X, Screen.Y);
          
            // draw the resize handle
            this.DrawRectangle("rgba(0,255,0,.5)", 10, 10, Screen.X + SWidth / 2, Screen.Y + SHeight / 2);
        }





        //------------------------------------------------------
        private void DrawShapes_Polygon(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryPolygon bp = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);

            PointF ScreenOffset = this.W2S(oHole.OffsetX, oHole.OffsetY);

            for (int i = 0; i < bp.PointList.Length; i++)
            {
                int FromIndex = i;
                int ToIndex = i + 1;
                if (ToIndex == bp.PointList.Length) ToIndex = 0; // Wrap at end

                Point3D FromPoint = bp.PointList[FromIndex];

                float fdx = FromPoint.X / CurrentZoom;
                float fdy = FromPoint.Y / CurrentZoom;

                Point3D ToPoint = bp.PointList[ToIndex];

                float tdx = ToPoint.X / CurrentZoom;
                float tdy = ToPoint.Y / CurrentZoom;

                PointF ScreenFrom = new PointF(ScreenOffset.X + fdx, ScreenOffset.Y + fdy);
                PointF ScreenTo = new PointF(ScreenOffset.X + tdx, ScreenOffset.Y + tdy);

                // Draw handles
                string color = "rgba(255,0,0,.5)"; 
                if (IsCurrentHole && i == MostRecentlySelectedPolygonVertexIndex)
                {
                    color = "#0000FF";
                }
                this.DrawCircle(color, 10, ScreenFrom.X, ScreenFrom.Y);

                PointF oCenter = EdgeCenter(ScreenFrom, ScreenTo);
                // Draw the handle for the edge
                color = "rgba(0,255,0,.5)";
                if (IsCurrentHole && i == MostRecentlySelectedPolygonEdgeIndex)
                {
                    color = "#0000FF";
                }
                this.DrawRectangle(color, 10, 10, oCenter.X, oCenter.Y);

                // Draw Line
                this.DrawLine("#0000FF", (float)0.5, ScreenFrom, ScreenTo);

                // Draw the move handle
                PointF oMove = PolygonCenter(bp);
                float mdx = oMove.X / CurrentZoom;
                float mdy = oMove.Y / CurrentZoom;

                oMove = new PointF(ScreenOffset.X + mdx, ScreenOffset.Y + mdy);
                color = IsCurrentHole ? "#0000FF" : "rgba(255,0,0,.5)";

                // Draw the move handle
                this.DrawCircle(color, 10, oMove.X, oMove.Y);
            }
        }






  
        //-------------------------------------------------------
        private void DrawShapes_Rectangle(LayoutHole oHole, bool IsCurrentHole)
        {
            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(oHole.HoleTypeIndex);

            PointF Screen = this.W2S(oHole.OffsetX, oHole.OffsetY);

            string color = IsCurrentHole ? "#0000FF" : "#ff0000";
            // Draw the move handle
            this.DrawCircle(color, 10, Screen.X, Screen.Y);

            // Adjust the width and height for zoom
            float SWidth = oRect.Width / CurrentZoom;
            float SHeight = oRect.Height / CurrentZoom;

            // Draw the rectangle as a set of lines so we can control how its positioned. The offset is the lower left
            color = "#0000FF";

            // The offset is to the lower left
            PointF LowerLeft = Screen;

            PointF LowerRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y);
            this.DrawLine(color, 1, LowerLeft, LowerRight);

            PointF UpperRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y - SHeight);
            this.DrawLine(color, 1, LowerRight, UpperRight);

            PointF UpperLeft = new PointF(LowerLeft.X, LowerLeft.Y - SHeight);
            this.DrawLine(color, 1, UpperRight, UpperLeft);

            // Last line
            this.DrawLine(color, 1, UpperLeft, LowerLeft);

            // draw the resize handle
            this.DrawRectangle("#00FF00", 10, 10, Screen.X + SWidth , Screen.Y - SHeight);
        }




        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            if (TryHoleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            return eMouseDownCapture.Nothing;
        }

        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (CurrentlySelectedHole != null)
            {
                UpdateCurrentHole(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                return true;
            }
            return false;
        }

        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Reset the currently selected items. Most recently selected remain active for any commands
             */
            CurrentlySelectedHole = null;
            CurrentlySelectedPolygonEdgeIndex = -1;
            CurrentlySelectedPolygonVertexIndex = -1;
        }


        public LayoutHole CurrentlySelectedHole { get; set; }
        public LayoutHole MostRecentlySelectedHole { get; set; }

        public eHandleType CurrentlySelectedHandleType { get; set; }

        public int CurrentlySelectedPolygonVertexIndex { get; set; }  // if this is a polygon, currently selected index
        public int MostRecentlySelectedPolygonVertexIndex { get; set; }

        public int CurrentlySelectedPolygonEdgeIndex { get; set; }  // if this is a polygon, currently selected edge
        public int MostRecentlySelectedPolygonEdgeIndex { get; set; }

        // This will manage a set of hole groups, of which there will be one that is active. All of the rectanges,
        // ellipses and polygons are in a list that the hole groups manage
        public List<HoleGroup> HoleGroupList { get; set; }

        public HoleGroup MostRecentlySelectedHoleGroup { get; set; }

        // The hole definitions are maintained in lists referenced by index. We do this this way so that this entire
        // structure can be serialized

        public List<BoundaryRectangle> BoundaryRectangleList { get; set; }
        public List<BoundaryEllipse> BoundaryEllipseList { get; set; }
        public List<BoundaryPolygon> BoundaryPolygonList { get; set; }

        // This shows holes over a specific edge, and we can show that edge as a shaded area if these values
        // are specified

        public float ShadeLength { get; set; }
        public float ShadeHeight { get; set; }



       
        public void RemoveCurrentPolygonVertex()
        {
            if (MostRecentlySelectedHole == null) return;

            if (MostRecentlySelectedHole.HoleType != "poly") return;

            if (MostRecentlySelectedPolygonVertexIndex == -1) return;

            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(MostRecentlySelectedHole.HoleTypeIndex);

            // Don't reduce to less than 3 points
            if (oPolygon.PointList.Length == 3) return;

            /*
             * Remove this point and clear the most recently selected edge
             */
#if DOTNET
            List<Point3D> tmp = new List<Point3D>(oPolygon.PointList);
            tmp.RemoveAt(MostRecentlySelectedPolygonVertexIndex);
            oPolygon.PointList = tmp.ToArray();
#else
            oPolygon.PointList.splice(this.MostRecentlySelectedPolygonVertexIndex, 1);
#endif

            MostRecentlySelectedPolygonVertexIndex = -1;

            // trigger a redraw

            DrawShapes();

          }



        public void RemoveHoleFromHoleGroup(HoleGroup hg, int index)
        {

#if DOTNET
            List<LayoutHole> tmp = new List<LayoutHole>(hg.HoleList);
            tmp.RemoveAt(index);
            hg.HoleList = tmp.ToArray();
#else
            hg.HoleList.splice(index,1);
#endif
        }



        public void SelectHoleGroup(string name)
        {
            for (int i=0; i < HoleGroupList.Count; i++)
            {
                HoleGroup hg = HoleGroupList.GetFrom(i);
                if (hg.HoleGroupID == name)
                {
                    MostRecentlySelectedHoleGroup = hg;
                    return;
                }
            }
        }



        // If the cur
        public void SplitCurrentPolygonEdge()
        {
            if (MostRecentlySelectedHole == null) return;

            if (MostRecentlySelectedHole.HoleType != "poly") return;

            if (MostRecentlySelectedPolygonEdgeIndex == -1) return;

            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(MostRecentlySelectedHole.HoleTypeIndex);
            /*
             * Locate this edge and create a new point in between the surrounding points
             */
            int FromIndex = MostRecentlySelectedPolygonEdgeIndex;
            int ToIndex = FromIndex + 1;

            if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

            Point3D FromPoint = oPolygon.PointList[FromIndex];
            Point3D ToPoint = oPolygon.PointList[ToIndex];

            PointF pF = new PointF(FromPoint.X, FromPoint.Y);
            PointF pT = new PointF(ToPoint.X, ToPoint.Y);

            PointF oCenter = EdgeCenter(pF, pT);

            Point3D pNew = new Point3D();
            pNew.X = oCenter.X;
            pNew.Y = oCenter.Y;
#if DOTNET
            List<Point3D> tmp = new List<Point3D>(oPolygon.PointList);
            tmp.Insert(FromIndex + 1, pNew);
            oPolygon.PointList = tmp.ToArray();
#else
            oPolygon.PointList.splice(FromIndex + 1,0,pNew);
#endif



            // trigger repaint
            DrawShapes();
        }







        /// <summary>
        /// Find the edge that's closest to the mouse X,Y
        /// </summary>
        /// <param name="ScreenMouseX"></param>
        /// <param name="ScreenMouseY"></param>
        /// <returns></returns>
        /// 
        protected bool TryHoleSelect(int ScreenMouseX, int ScreenMouseY)
        {
            for (int i=0; i < GetHoleListLength(MostRecentlySelectedHoleGroup.HoleList); i++)
            {
                LayoutHole oHole = MostRecentlySelectedHoleGroup.HoleList[i];
                PointF MoveHandle = new PointF(0, 0);
                PointF ResizeHandle = new PointF(0, 0);

                switch (oHole.HoleType)
                {
                    case "rect":
                        if (TrySelectRectangle(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                    case "poly":
                        if (TrySelectPolygon(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                    case "ell":
                        if (TrySelectEllipse(oHole, ScreenMouseX, ScreenMouseY)) return true;
                        break;
                }
            }

            return false;
        }
 

        // See if there is a hit on the target point
        private bool IsHitOnTarget(int ScreenMouseX, int ScreenMouseY, PointF Target)
        {
            double distance = Math.sqrt((ScreenMouseX - Target.X) * (ScreenMouseX - Target.X) +
                                                      (ScreenMouseY - Target.Y) * (ScreenMouseY - Target.Y));

            return distance < 10;
        }


        //----------------------------------------------------------------------------
        private bool TrySelectEllipse(LayoutHole oHole,int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            PointF ResizeHandle = new PointF(0, 0);

            BoundaryEllipse oEllipse = BoundaryEllipseList.GetFrom(oHole.HoleTypeIndex);

            // The move handle  is the center
            MoveHandle = this.W2S(oHole.OffsetX, oHole.OffsetY);
            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, MoveHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }

            // The resize handle is the lower right corner
            ResizeHandle = this.W2S(oHole.OffsetX + oEllipse.Width / 2, oHole.OffsetY + oEllipse.Height / 2);
            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, ResizeHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.ResizeHandle;
                return true;
            }

            return false;
        }


 



        //--------------------------------------------------------------------------
        /*
         * A Polygon can be selected in any of these ways:
         * 1. A vertex can be selected, which makes it the active vertex
         * 2. An edge can be selected, which makes it the active edge
         * 3. The move handle can be selected. The move handle is generated as the average of the points of the polygon
         */
        private bool TrySelectPolygon(LayoutHole oHole, int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            float dx;
            float dy;


            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(oHole.HoleTypeIndex);

            PointF ScreenOffset = this.W2S(oHole.OffsetX, oHole.OffsetY);
            /*
             * See if this is one of the vertices
             */
            for (int i=0; i < oPolygon.PointList.Length; i++)
            {
                Point3D p = oPolygon.PointList[i];
                dx = p.X / CurrentZoom;
                dy = p.Y / CurrentZoom;

                PointF Target = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, Target))
                {
                    CurrentlySelectedHole = oHole;
                    // Most recently selected is used for commands that happen after the mouse is up
                    MostRecentlySelectedHole = oHole;
                    CurrentlySelectedHandleType = eHandleType.VertexHandle;

                    CurrentlySelectedPolygonVertexIndex = i;
                    // The most recently used is for commands that happen after the mouse is up
                    MostRecentlySelectedPolygonVertexIndex = i;

                    return true;
                }
            }
            /*
             * See if this is one of the edges
             */
            for (int i = 0; i < oPolygon.PointList.Length; i++)
            {
                int FromIndex = i;
                int ToIndex = i + 1;
                if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

                Point3D FromPoint = oPolygon.PointList[FromIndex];

                dx = FromPoint.X / CurrentZoom;
                dy = FromPoint.Y / CurrentZoom;

                PointF pFrom = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                Point3D ToPoint = oPolygon.PointList[ToIndex];

                dx = ToPoint.X / CurrentZoom;
                dy = ToPoint.Y / CurrentZoom;

                PointF pTo = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

                PointF Target = EdgeCenter(pFrom, pTo);

                if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, Target))
                {
                    CurrentlySelectedHole = oHole;
                    // Most recently selected is used for commands that happen after the mouse is up
                    MostRecentlySelectedHole = oHole;
                    CurrentlySelectedHandleType = eHandleType.EdgeHandle;

                    CurrentlySelectedPolygonEdgeIndex = i;
                    // The most recently used is for commands that happen after the mouse is up
                    MostRecentlySelectedPolygonEdgeIndex = i;

                    return true;
                }
            }
            /*
             * See if this is the move handle at the center of the polygon
             */
            PointF pCenter = PolygonCenter(oPolygon);
            dx = pCenter.X / CurrentZoom;
            dy = pCenter.Y / CurrentZoom;
            pCenter = new PointF(ScreenOffset.X + dx, ScreenOffset.Y + dy);

            if (IsHitOnTarget(ScreenMouseX,ScreenMouseY,pCenter))
            {
                CurrentlySelectedHole = oHole;
                // Most recently selected is used for commands that happen after the mouse is up
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }


            return false;
        }

        private PointF PolygonCenter(BoundaryPolygon oPolygon)
        {
            float x = 0;
            float y = 0;
            for (int i=0; i < oPolygon.PointList.Length; i++)
            {
                Point3D p = oPolygon.PointList[i];
                x += p.X;
                y += p.Y;
            }

            PointF oCenter = new PointF(x / oPolygon.PointList.Length, y / oPolygon.PointList.Length);

            return oCenter;

        }






        //----------------------------------------------------------------------------
        private bool TrySelectRectangle(LayoutHole oHole,int ScreenMouseX, int ScreenMouseY)
        {
            PointF MoveHandle = new PointF(0, 0);
            PointF ResizeHandle = new PointF(0, 0);

            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(oHole.HoleTypeIndex);

            // The move handle  is the lower left
            MoveHandle = this.W2S(oHole.OffsetX, oHole.OffsetY);
            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, MoveHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }

            // The resize handle is the lower right corner
            // the position of the handle is based on the zoom

            float HandleOffsetX  = oRect.Width / this.CurrentZoom;
            float HandleOffsetY = oRect.Height / this.CurrentZoom;
            ResizeHandle = new PointF(MoveHandle.X + HandleOffsetX, MoveHandle.Y - HandleOffsetY);

            if (IsHitOnTarget(ScreenMouseX, ScreenMouseY, ResizeHandle))
            {
                CurrentlySelectedHole = oHole;
                MostRecentlySelectedHole = oHole;
                CurrentlySelectedHandleType = eHandleType.ResizeHandle;
                return true;
            }

            return false;
        }


 



        public eOperationStatus UpdateCurrentHole(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (CurrentlySelectedHole == null) return eOperationStatus.NoVertexSelected;

            switch (CurrentlySelectedHole.HoleType)
            {
                case "rect":
                    UpdateRectangle(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
                case "poly":
                    UpdatePolygon(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
                case "ell":
                    UpdateEllipse(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
                    break;
            }
   

            return eOperationStatus.OK;
        }





        private void UpdateEllipse(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            BoundaryEllipse oEllipse = BoundaryEllipseList.GetFrom(CurrentlySelectedHole.HoleTypeIndex);
            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if (this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            switch (CurrentlySelectedHandleType)
            {
                case eHandleType.MoveHandle:

                    CurrentlySelectedHole.OffsetX = newx;
                    CurrentlySelectedHole.OffsetY = newy;
                    break;
                case eHandleType.ResizeHandle:
                    // Get the hole struct and change the size
                    switch (CurrentlySelectedHole.HoleType)
                    {
                        case "ell":
                            oEllipse.Width += ScreenDeltaX * 2;
                            oEllipse.Height += ScreenDeltaY * 2;
                            break;
                    }
                    break;
            }
        }






        //------------------------------------------------------------------------------------------------
        protected void UpdatePolygon(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            BoundaryPolygon oPolygon = BoundaryPolygonList.GetFrom(CurrentlySelectedHole.HoleTypeIndex);
            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);
            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if (this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            switch (CurrentlySelectedHandleType)
            {
                case eHandleType.VertexHandle:
                    /*
                     * Retrieve this vertex and update 
                     */
                     Point3D p = oPolygon.PointList[CurrentlySelectedPolygonVertexIndex];

                    p.X += ScreenDeltaX * CurrentZoom;
                    p.Y += ScreenDeltaY * CurrentZoom;

                    break;
                case eHandleType.EdgeHandle:
                    /*
                     * The points associated with the edge are the ones at the value and after
                     */
                    int FromIndex = CurrentlySelectedPolygonEdgeIndex;
                    int ToIndex = FromIndex + 1;
                    if (ToIndex == oPolygon.PointList.Length) ToIndex = 0;

                    Point3D FromPoint = oPolygon.PointList[FromIndex];
                    Point3D ToPoint = oPolygon.PointList[ToIndex];

                    FromPoint.X += ScreenDeltaX * CurrentZoom;
                    FromPoint.Y += ScreenDeltaY * CurrentZoom;

                    ToPoint.X += ScreenDeltaX * CurrentZoom;
                    ToPoint.Y += ScreenDeltaY * CurrentZoom;
                    break;
                case eHandleType.MoveHandle:
                    /*
                     * Update the offset to move the whole shape
                     */
                    CurrentlySelectedHole.OffsetX = newx;
                    CurrentlySelectedHole.OffsetY = newy;
                    /*
                     * Update all points with the delta
                     */
#if false
                    for (int i=0; i < oPolygon.PointList.Length; i++)
                    {
                        Point3D pMove = oPolygon.PointList[i];
                        pMove.X += ScreenDeltaX;
                        pMove.Y += ScreenDeltaY;
                    }
#endif
                    break;
            }
        }





        private void UpdateRectangle(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            BoundaryRectangle oRect = BoundaryRectangleList.GetFrom(CurrentlySelectedHole.HoleTypeIndex);
            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if (this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            switch (CurrentlySelectedHandleType)
            {
                case eHandleType.MoveHandle:

                    CurrentlySelectedHole.OffsetX = newx;
                    CurrentlySelectedHole.OffsetY = newy;
                    break;
                case eHandleType.ResizeHandle:
                    // Get the hole struct and change the size
                    switch (CurrentlySelectedHole.HoleType)
                    {
                        case "rect":
                            oRect.Width += ScreenDeltaX * CurrentZoom;
                            oRect.Height -= ScreenDeltaY * CurrentZoom;
                            break;
                    }
                    break;
            }
        }





}
public class Edit2DRectangleWithOffset :Edit2DBase
{
        public Edit2DRectangleWithOffset()
        {
            //  This is a trick to get code to show up when pasting into typescript.
            // Because this method is extracted from the class by a program that skips the top of the file
            // the #define won't be picked up. At the same time when the typescript paste occurs the
            // compiler will evaluate the #define and generate the 'else', which will be appropriate for javascript.
#if DOTNET
#else
            super();
#endif

            moveHandle = new Vertex();
            moveHandle.X = 0;
            moveHandle.Y = 0;

            Offset = new Point3D();
            Offset.X = 0;
            Offset.Y = 0;

            oRect = new BoundaryRectangle();
            oRect.Width = 20;
            oRect.Height = 20;

            resizeHandle = new Vertex();
            resizeHandle.X = oRect.Width;
            resizeHandle.Y = -oRect.Height;

            CurrentlySelectedVertex = null;
            MostRecentlySelectedVertex = null;
        }


        /*
         * Sub classes will override these methods to implement mouse operations on their canvas
         */
       override  public eMouseDownCapture MouseDown(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Try to select a vertex handle first, then an edge handle. The order actually isn't important,
             * only that we try to select one or the other
             */
            if (TryHandleSelect(ScreenMouseX, ScreenMouseY)) return eMouseDownCapture.VertexHandle;

            return eMouseDownCapture.Nothing;
        }

   



        override public bool MouseMove(eMouseState MouseState, int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            if (CurrentlySelectedVertex == null) return false;

            UpdateRectangle(ScreenMouseX, ScreenMouseY, ScreenDeltaX, ScreenDeltaY);
            
            return true;
        }




        override public void MouseUp(int ScreenMouseX, int ScreenMouseY)
        {
            /*
             * Reset the current handle
             */
            CurrentlySelectedVertex = null;
        }




        // ---------------------------------------------------
        public override void DrawShapes()
        {
            this.Clear("#FFFFFF");
      
            DrawGrid();

            DrawAxis();

            PointF pMove = this.W2S(moveHandle.X, moveHandle.Y);
            this.DrawCircle("#ff0000", 10, pMove.X + 5, pMove.Y + 5);

            PointF Screen = this.W2S(Offset.X, Offset.Y);

            // Adjust the width and height for zoom
            float SWidth = oRect.Width / CurrentZoom;
            float SHeight = oRect.Height / CurrentZoom;

            // Draw the rectangle as a set of lines so we can control how its positioned. The offset is the lower left
            string color = "#0000FF";

            // The offset is to the lower left
            PointF LowerLeft = Screen;

            PointF LowerRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y);
            this.DrawLine(color, 1, LowerLeft, LowerRight);

            PointF UpperRight = new PointF(LowerLeft.X + SWidth, LowerLeft.Y - SHeight);
            this.DrawLine(color, 1, LowerRight, UpperRight);

            PointF UpperLeft = new PointF(LowerLeft.X, LowerLeft.Y - SHeight);
            this.DrawLine(color, 1, UpperRight, UpperLeft);

            // Last line
            this.DrawLine(color, 1, UpperLeft, LowerLeft);

            // draw the resize handle

            PointF pResize = this.W2S(resizeHandle.X, resizeHandle.Y);
            this.DrawRectangle("#00FF00", 10, 10, pResize.X , pResize.Y);

        }



        public eHandleType CurrentlySelectedHandleType { get; set; }

        public Vertex moveHandle;
        public Vertex resizeHandle;

        /// <summary>
        /// Offset of the rectangle
        /// </summary>
        Point3D Offset;
        /// <summary>
        /// Width and height of the rectangle
        /// </summary>
        BoundaryRectangle oRect;


        Vertex CurrentlySelectedVertex;
        Vertex MostRecentlySelectedVertex;




        public bool TryHandleSelect(int ScreenMouseX, int ScreenMouseY)
        {
            float MoveDistance = GetDistance(moveHandle, ScreenMouseX, ScreenMouseY);

            if (MoveDistance < 10)
            {
                // For purposes of dragging the handle we want to know the currently selected handle. However, its 
                // also useful to remember the most recently selected handle for purposes of operations

                CurrentlySelectedVertex = moveHandle;
                MostRecentlySelectedVertex = moveHandle;
                CurrentlySelectedHandleType = eHandleType.MoveHandle;
                return true;
            }

            float ResizeDistance = GetDistance(resizeHandle, ScreenMouseX, ScreenMouseY);

            if (ResizeDistance < 10)
            {
                // For purposes of dragging the handle we want to know the currently selected handle. However, its 
                // also useful to remember the most recently selected handle for purposes of operations

                CurrentlySelectedVertex = resizeHandle;
                MostRecentlySelectedVertex = resizeHandle;
                CurrentlySelectedHandleType = eHandleType.ResizeHandle;
                return true;
            }

            return false;   // no handle selected
        }

        private float GetDistance(Vertex vHandle, int ScreenMouseX, int ScreenMouseY)
        {
            PointF ScreenCoordinates = this.W2S(vHandle.X, vHandle.Y);

            float distance = (float) Math.sqrt(
                    (ScreenMouseX - ScreenCoordinates.X) * (ScreenMouseX - ScreenCoordinates.X) +
                    (ScreenMouseY - ScreenCoordinates.Y) * (ScreenMouseY - ScreenCoordinates.Y));

            return distance;
        }



        public eOperationStatus UpdateCurrentHandleToScreenPoint(int ScreenMouseX, int ScreenMouseY)
        {

            if (CurrentlySelectedVertex == null) return eOperationStatus.NoVertexSelected;

            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if ( this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            CurrentlySelectedVertex.X = (float) newx;
            CurrentlySelectedVertex.Y = (float) newy;    

            return eOperationStatus.OK;
        }




        private void UpdateRectangle(int ScreenMouseX, int ScreenMouseY, int ScreenDeltaX, int ScreenDeltaY)
        {
            PointF pWorld = this.S2W(ScreenMouseX, ScreenMouseY);

            float newx = pWorld.X;
            float newy = pWorld.Y;

            // Update the world points to the nearest grid
            if (this.GridSize > 1)
            {
                newx = this.RoundToGrid((int)pWorld.X);
                newy = this.RoundToGrid((int)pWorld.Y);
            }

            switch (CurrentlySelectedHandleType)
            {
                case eHandleType.MoveHandle:
                    moveHandle.X = newx;
                    moveHandle.Y = newy;

                    resizeHandle.X += ScreenDeltaX;
                    resizeHandle.Y += ScreenDeltaY;

                    Offset.X = newx;
                    Offset.Y = newy;
                    break;
                case eHandleType.ResizeHandle:
                    resizeHandle.X += ScreenDeltaX;
                    resizeHandle.Y += ScreenDeltaY;
                    // Get the hole struct and change the size
                    oRect.Width += ScreenDeltaX * CurrentZoom;
                    oRect.Height -= ScreenDeltaY * CurrentZoom;
                    break;
            }
        }



}
public class SVector2
{
        
        public float X { get; set; }
        public float Y { get; set; }

        public SVector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.sqrt(Y * Y + X * X);
            }
        }

        public static SVector2 Add(SVector2 v1, SVector2 v2)
        {
            return new SVector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static SVector2 Subtract(SVector2 v1, SVector2 v2)
        {
            return new SVector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public SVector2 Negate(SVector2 v1)
        {
            return new SVector2(-v1.X, -v1.Y);
        }

        public static SVector2 Scale(SVector2 v2, float scale)
        {
            return new SVector2(v2.X * scale, v2.Y * scale);
        }

        public static SVector2 Normalize(SVector2 v1)
        {
            float inverse = 1 / v1.Magnitude;

            return new SVector2(v1.X * inverse, v1.Y * inverse);
        }

        public static double Distance(SVector2 v1, SVector2 v2)
        {
            return
               Math.sqrt
               (
                   (v1.X - v2.X) * (v1.X - v2.X) +
                   (v1.Y - v2.Y) * (v1.Y - v2.Y) 
               );
        }

        public static SVector2 Interpolate(
            SVector2 v1,
            SVector2 v2,
            float control)
        {
            return new SVector2(
                v1.X * (1 - control) + v2.X * control,
                v1.Y * (1 - control) + v2.Y * control
                );
        }


}
    public partial class BoundaryEllipse 
{
#if DOTNET
#else
        // This class doesn't inherit in typescript, we need to give it the type property
        public string BoundaryType { get; set; } = "";
#endif
        /// <summary>
        /// Width of Ellipse
        /// </summary>
        [HelpProperty(SampleValue ="30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Width { get; set; } = 20;
        /// <summary>
        /// Height of ellipse
        /// </summary>
        [HelpProperty(SampleValue ="30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Height { get; set; } = 20;
        /// <summary>
        /// ZDepth of Ellipse when placing this shape, either as a boundary or as a hole. Using ZDepth as part of the 
        /// a boundary for a panel mesh lets you displace the mesh and create separation. Varying the Z Depth for a hole can produce
        /// interesting effects and variations on shapes but should be done with caution as it can easily cause a shape to 'break'
        /// or otherwise render as something unrecognizable.
        /// </summary>
        [HelpProperty(SampleValue ="0", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float ZDepth { get; set; } = 0;


        /// <summary>
        /// Constructor
        /// </summary>
        public BoundaryEllipse()
        {
            // Set the type in the base class, the JSON serialization will use this
            this.BoundaryType = "ellipse";
        }

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object

        public static BoundaryEllipse CopyFrom(BoundaryEllipse oFrom)
        {
            BoundaryEllipse oEllipse = new BoundaryEllipse();
            oEllipse.Width = oFrom.Width;
            oEllipse.Height = oFrom.Height;
            oEllipse.ZDepth = oFrom.ZDepth;

            return oEllipse;
        }



}
    public partial class BoundaryPolygon 
{
#if DOTNET
#else
        // This class doesn't inherit in typescript, we need to give it the type property
        public string BoundaryType { get; set; } = "";
#endif
        /// <summary>
        /// List of points for the boundarylinesegment
        /// </summary>
        [HelpProperty("pointlist")]

        // The reason that this is an array instead of a list is that its a data structure that may be exported to and imported
        // from JSON, and this data structure may be referenced both in JS and in C# so its best to keep it the same

        public Point3D[] PointList { get; set; } = new Point3D[0];

        public BoundaryPolygon()
        {
            this.BoundaryType = "polygon";
        }

        

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object
        public static BoundaryPolygon CopyFrom(BoundaryPolygon oFrom)
        {
            BoundaryPolygon oPolygon = new BoundaryPolygon();
            oPolygon.PointList = new Point3D[oFrom.PointList.Length];
            for (int i=0; i < oFrom.PointList.Length; i++)
            {
                Point3D p = oFrom.PointList[i];
                oPolygon.PointList[i] = Point3D.CopyFrom(p);
            }

            return oPolygon;
        }




}
    public partial class BoundaryRectangle 
{
#if DOTNET
#else
        // This class doesn't inherit in typescript, we need to give it the type property
        public string BoundaryType { get; set; } = "";
#endif
        public BoundaryRectangle()
        {
            // Set the type in the base class, the JSON serialization will use this
            this.BoundaryType = "rectangle";
        }
        /// <summary>
        /// Width of rectangle
        /// </summary>
        [HelpProperty(SampleValue = "30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Width { get; set; } = 20;

        /// <summary>
        /// Height of rectangle
        /// </summary>
        [HelpProperty(SampleValue = "30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Height { get; set; } = 20;

        /// <summary>
        /// ZDepth of Rectangle when placing this shape, either as a boundary or as a hole. Using ZDepth as part of the 
        /// a boundary for a panel mesh lets you displace the mesh and create separation. Varying the Z Depth for a hole can produce
        /// interesting effects and variations on shapes but should be done with caution as it can easily cause a shape to 'break'
        /// or otherwise render as something unrecognizable.
        /// </summary>
        [HelpProperty(SampleValue = "0", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float ZDepth { get; set; } = 0;


        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object

        public static BoundaryRectangle CopyFrom(BoundaryRectangle oFrom)
        {
            BoundaryRectangle oRectangle = new BoundaryRectangle();
            oRectangle.Width = oFrom.Width;
            oRectangle.Height = oFrom.Height;
            oRectangle.ZDepth = oFrom.ZDepth;

            return oRectangle;
        }







}
    public partial class BoundaryRoot 
{
        // This value is set by the subclass
        public string BoundaryType { get; set; } = "";

        public BoundaryRoot()
        {
            this.BoundaryType = "";
        }

        public static BoundaryRoot CopyFrom(BoundaryRoot oFrom)
        {
            switch (oFrom.BoundaryType)
            {
                case "rectangle":
                    return BoundaryRectangle.CopyFrom((BoundaryRectangle)oFrom);
                case "ellipse":
                    return BoundaryEllipse.CopyFrom((BoundaryEllipse)oFrom);
                case "polygon":
                    return BoundaryPolygon.CopyFrom((BoundaryPolygon)oFrom);
            }
            return new BoundaryRoot();
        }


}
    public partial class Hole
{
        public Hole()
        {
            Offset = new Point3D();
        }
        /// <summary>
        /// The ID value identifies this hole to other structures that may need it. It is not required
        /// </summary>
        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue = "Window")]
        public string ID { get; set; } = "";             // this is used to identify this hole structure with a larger shape

        /// <summary>
        /// The offset of a hole is where it is positioned
        /// </summary>
        [HelpProperty]
        public Point3D Offset { get; set; } = new Point3D(0, 0, 0);

        /// <summary>
        /// The boundary for a hole can be a rectangle, ellipse or line segment
        /// </summary>
        [HelpProperty]
        public BoundaryRoot Boundary { get; set; }

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object

        public static Hole CopyFrom(Hole oFrom)
        {
            Hole oHole = new Hole();

            oHole.ID = oFrom.ID;
            oHole.Offset = Point3D.CopyFrom(oFrom.Offset);
            oHole.Boundary = BoundaryRoot.CopyFrom(oFrom.Boundary);

            return oHole;
        }


}
    public partial class HoleContainer 
{
        /// <summary>
        /// List of Holes
        /// </summary>
        [HelpProperty("HoleContainer")]

        // The reason that this is an array instead of a list is that its a data structure that may be exported to and imported
        // from JSON, and this data structure may be referenced both in JS and in C# so its best to keep it the same

        public Hole[] HoleList { get; set; } = new Hole[0];


        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object
        public static HoleContainer CopyFrom(HoleContainer oFrom)
        {
            HoleContainer oOutline = new HoleContainer();
            oOutline.HoleList = new Hole[oFrom.HoleList.Length];
            for (int i=0; i < oFrom.HoleList.Length; i++)
            {
                Hole p = oFrom.HoleList[i];
                oOutline.HoleList[i] = Hole.CopyFrom(p);
            }

            return oOutline;
        }




}
    public partial class JSONDataCarriage
{
            public string fieldname { get; set; }
            public string fieldvalue { get; set; }  // string encoded json


}
    public partial class JSONDataTrain
{
        public JSONDataCarriage[] JSONDataCarriageArray { get; set; }

        public JSONDataTrain()
        {
            JSONDataCarriageArray = new JSONDataCarriage[0];
        }
       


}
    public partial class Point2D 
{
        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue ="13.4")]
        public float X { get; set; }
        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue = "2")]
        public float Y { get; set; }

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object

        public static Point2D CopyFrom(Point2D oFrom)
        {
            Point2D oPoint = new Point2D();

            oPoint.X = oFrom.X;
            oPoint.Y = oFrom.Y;

            return oPoint;
        }




}
    public partial class Point2DContainer 
{
        /// <summary>
        /// List of points for a 2D outline.
        /// </summary>
        [HelpProperty("Point2DContainer")]

        // The reason that this is an array instead of a list is that its a data structure that may be exported to and imported
        // from JSON, and this data structure may be referenced both in JS and in C# so its best to keep it the same

        public Point2D[] Point2DList { get; set; } = new Point2D[0];


        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object
        public static Point2DContainer CopyFrom(Point2DContainer oFrom)
        {
            Point2DContainer oOutline = new Point2DContainer();
            oOutline.Point2DList = new Point2D[oFrom.Point2DList.Length];
            for (int i=0; i < oFrom.Point2DList.Length; i++)
            {
                Point2D p = oFrom.Point2DList[i];
                oOutline.Point2DList[i] = Point2D.CopyFrom(p);
            }

            return oOutline;
        }




}
    public partial class Point3D 
{
        /// <summary>
        /// Constructor
        /// </summary>
        public Point3D()
        {

        }

        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue ="13.4")]
        public float X { get; set; }
        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue = "2")]
        public float Y { get; set; }
        [HelpProperty(XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent, SampleValue = "-34.2")]
        public float Z { get; set; }

      
        public static Point3D CopyFrom(Point3D oFrom)
        {
            Point3D p = new Point3D();
            p.X = oFrom.X;
            p.Y = oFrom.Y;
            p.Z = oFrom.Z;

            return p;
        }



}
    public partial class Edge
{
        /// <summary>
        /// An edge can be identified by an ID value for purposes of code that manages the layout
        /// </summary>
        [HelpProperty(SampleValue = "SideEdge", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public string ID { get; set; } = "";

        /// <summary>
        /// The wall resulting from an edge can have a set of holes, and this ID value is used to identify them
        /// </summary>
        [HelpProperty(SampleValue = "hgSideWallWindows", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public string HoleGroupID { get; set; } = "";

        /// <summary>
        /// First endpoint of this edge. This the index value of the vertex object. The edge keeps the indices of the points, not the x,y values.
        /// </summary>
        [HelpProperty(SampleValue = "3", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int p1 { get; set; }

        /// <summary>
        /// Second endpoint of this edge. This the index value of the vertex object. The edge keeps the indices of the points, not the x,y values.
        /// </summary>
        [HelpProperty(SampleValue = "3", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int p2 { get; set; }

        /// <summary>
        /// Width of this edge. This is how thick the vertical wall is
        /// </summary>
        [HelpProperty(SampleValue = "20", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int Width { get; set; } = 5;
        /// <summary>
        /// Height of this edge. This is the vertical height of the wall created by this edge
        /// </summary>
        [HelpProperty(SampleValue = "20", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int Height { get; set; } = 20;

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object
         public static Edge CopyFrom(Edge oSource)
        {
            Edge e = new Edge();
            e.ID = oSource.ID;
            e.HoleGroupID = oSource.HoleGroupID;
            e.p1 = oSource.p1;
            e.p2 = oSource.p2;
            e.Width = oSource.Width;
            e.Height = oSource.Height;

            return e;
        }




    
    

}
    public partial class HoleGroup
{
        public HoleGroup()
        {
            HoleGroupID = "";
            HoleList = new LayoutHole[0];
        }
        /// <summary>
        /// ID of this holegroup. This ID value is used to tie this hole group to a panel. Hole groups can be shared amongst different 
        /// panels in a layout.
        /// </summary>
        [HelpProperty(SampleValue = "hgFrontPanel", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public string HoleGroupID { get; set; }
        /// <summary>
        /// Array of Holes. The Hole descriptors has an offset, a type, and an index into the description of the hole specifics
        /// </summary>
        [HelpProperty(SampleValue = "", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public LayoutHole[] HoleList { get; set; }


}
    public partial class HorizontalPanel
{
        public HorizontalPanel()
        {
            Height = 0;
            Thickness = 5;
            DescriptorList = new string[0];
            HoleGroupID = "";
        }
        /// <summary>
        /// Height to place this panel. Wall panels normally have a base height of 0.
        /// </summary>
        [HelpProperty(SampleValue = "40", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Height { get; set; }

        /// <summary>
        /// Thickness of this panel. 
        /// </summary>
        [HelpProperty(SampleValue = "4", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Thickness { get; set; }

        /// <summary>
        /// Descriptor list. This is a list of points that make up the horizontal panel. The descriptor has 3 components.
        /// The first component is which edge this point is associated with. The second component is the position of the point,
        /// the inside edge, the center edge and the outside edge. The third component is which point on the edge, the left or
        /// right point. Left and Right are defined relative to the front of the edge. The front of the edge is defined
        /// by its points p1 and p2. You face the front of the edge, p1 is on the left and p2 is on the right.
        /// </summary>
        [HelpProperty(SampleValue = "", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.XElement)]
        public string[] DescriptorList { get; set; }

        /// <summary>
        /// The horizontal panel can have holes. This is the ID of the HoleGroup within the SimpleLayout that contains
        /// the hole definitions.
        /// </summary>
        [HelpProperty(SampleValue = "40", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public string HoleGroupID { get; set; }


}
    public partial class LayoutHole
{
        /// <summary>
        /// X Offset of this hole. 
        /// </summary>
        [HelpProperty(SampleValue = "30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float OffsetX { get; set; }
        /// <summary>
        /// Y Offset of this hole. 
        /// </summary>
        [HelpProperty(SampleValue = "30", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float OffsetY { get; set; }

        /// <summary>
        /// The type of the hole indicates whether its a rectangle, ellipse or polygon
        /// </summary>
        [HelpProperty(SampleValue = "rect", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public string HoleType { get; set; }

        /// <summary>
        /// Within a layout the outlines for the rectangle, ellipse and polygon are stored in separate arrays. This index value
        /// is the index value of the specific outline within its array
        /// </summary>
        [HelpProperty(SampleValue = "4", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int HoleTypeIndex { get; set; }

        public LayoutHole()
        {

        }

        public static LayoutHole CopyFrom(LayoutHole oFrom)
        {
            LayoutHole oHole = new LayoutHole();
            oHole.OffsetX = oFrom.OffsetX;
            oHole.OffsetY = oFrom.OffsetY;

            oHole.HoleType = oFrom.HoleType;
            oHole.HoleTypeIndex = oFrom.HoleTypeIndex;

            return oHole;
        }



}
    public partial class Vertex
{

        public Vertex()
        {
            Index = 0;
            X = 0;
            Y = 0;
        }
        /// <summary>
        /// Index of this vertex. Edges can share a vertex, so vertices are maintained in a list by the layout and referred to via an index value
        /// </summary>
        [HelpProperty(SampleValue = "5", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public int Index { get; set; }

        /// <summary>
        /// X value of this vertex. A vertex is defined in a 2D coordinate system looking down onto the network of walls/edges
        /// </summary>
        [HelpProperty(SampleValue = "25", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float X { get; set; }

        /// <summary>
        /// Y value of this vertex. A vertex is defined in a 2D coordinate system looking down onto the network of walls/edges
        /// </summary>
        ///   
        [HelpProperty(SampleValue = "25", XPropertyPosition = HelpPropertyAttribute.eXPropertyPosition.AttributeOfParent)]
        public float Y { get; set; }

        // The CopyFrom method is used in Javascript to convert a version of the object retrieved via JSON
        // into a full object of this type. An object retrieved via JSON.parse() will have the correct property
        // names but not be an object of this type, this will take that property-only version and create a full
        // object
        public static Vertex CopyFrom(Vertex oSource)
        {
            Vertex v = new Vertex();

            v.Index = oSource.Index;
            v.X = oSource.X;
            v.Y = oSource.Y;

            return v;
        }


}

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


