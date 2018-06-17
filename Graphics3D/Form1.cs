using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Graphics3D
{
    public partial class Form1 : Form
    {
        public Bitmap bmp;
        public Bitmap bmptemp;
        public Graphics graphics;
        public Pen pen;
        public int length = 50;
        public int height = 25;
        public int difference = -60;
        public Point4D[] originalVertices1;
        public Point4D[] vertices1;
        public face[] faces1;
        public Point4D[] originalVertices2;
        public Point4D[] vertices2;
        public face[] faces2;
        public float alfa = 30;
        public float beta = 0;
        public float[,] zBuffer;
        float distance = 300;
        public Color[] colors1;
        public Color[] colors2;

        public class face
        {
            public Point4D[] vertices;
            Point4D normal;
            Point4D[] triangle1;
            Point4D[] triangle2;
            public Color color;

            public face(Point4D v1, Point4D v2, Point4D v3, Point4D v4, Point4D n, Color c)
            {
                vertices = new Point4D[4];
                vertices[0] = v1;
                vertices[1] = v2;
                vertices[2] = v3;
                vertices[3] = v4;
                normal = n;
                triangle1 = new Point4D[3] { vertices[0], vertices[1], vertices[2] };
                triangle2 = new Point4D[3] { vertices[0], vertices[2], vertices[3] };
                color = c;
            }
        }

        public class ETentry
        {
            public int ymin { get; set; }
            public int ymax { get; set; }
            public float xmin { get; set; }
            public float zmin { get; set; }
            public float oneoverm { get; set; }
            public float dzOVERdy { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = null;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            graphics = pictureBox1.CreateGraphics();
            pen = new Pen(Color.White, 3);
            originalVertices1 = new Point4D[8];
            vertices1 = new Point4D[8];
            originalVertices1[0] = new Point4D(-length / 2, -height, -length / 2, 1);
            originalVertices1[1] = new Point4D(length / 2, -height, -length / 2, 1);
            originalVertices1[2] = new Point4D(length / 2, -height, length / 2, 1);
            originalVertices1[3] = new Point4D(-length / 2, -height, length / 2, 1);
            originalVertices1[4] = new Point4D(-length / 2, height, length / 2, 1);
            originalVertices1[5] = new Point4D(-length / 2, height, -length / 2, 1);
            originalVertices1[6] = new Point4D(length / 2, height, -length / 2, 1);
            originalVertices1[7] = new Point4D(length / 2, height, length / 2, 1);
            faces1 = new face[6];

            originalVertices2 = new Point4D[8];
            vertices2 = new Point4D[8];
            originalVertices2[0] = new Point4D(-length / 2 + difference, -height + difference, -length / 2 + difference, 1);
            originalVertices2[1] = new Point4D(length / 2 + difference, -height + difference, -length / 2 + difference, 1);
            originalVertices2[2] = new Point4D(length / 2 + difference, -height + difference, length / 2 + difference, 1);
            originalVertices2[3] = new Point4D(-length / 2 + difference, -height + difference, length / 2 + difference, 1);
            originalVertices2[4] = new Point4D(-length / 2 + difference, height + difference, length / 2 + difference, 1);
            originalVertices2[5] = new Point4D(-length / 2 + difference, height + difference, -length / 2 + difference, 1);
            originalVertices2[6] = new Point4D(length / 2 + difference, height + difference, -length / 2 + difference, 1);
            originalVertices2[7] = new Point4D(length / 2 + difference, height + difference, length / 2 + difference, 1);
            faces2 = new face[6];

            zBuffer = new float[pictureBox1.Width, pictureBox1.Height];           

            colors1 = new Color[6] { Color.White, Color.Red, Color.Gray, Color.Green, Color.Blue, Color.Pink };
            colors2 = new Color[6] { Color.Green, Color.Blue, Color.Pink, Color.White, Color.Red, Color.Gray};
        }

        private void drawCoordButton_Click(object sender, EventArgs e)
        {
            clearZBuffer();
            bmptemp = new Bitmap(pictureBox1.Width, pictureBox1.Height);          
            bmptemp = drawCube(originalVertices2, vertices2, distance, faces2, 240, bmptemp, colors2);
            bmptemp = drawCube(originalVertices1, vertices1, distance, faces1, 240, bmptemp, colors1);
            pictureBox1.Image = bmptemp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                alfa += 30;
                clearZBuffer();
                bmptemp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                bmptemp = drawCube(originalVertices2, vertices2, distance, faces2, 240, bmptemp, colors2);
                bmptemp = drawCube(originalVertices1, vertices1, distance, faces1, 240, bmptemp, colors1);
                pictureBox1.Image = bmptemp;
            }
            else if(e.Button == MouseButtons.Right)
            {
                beta += 30;
                clearZBuffer();
                bmptemp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                bmptemp = drawCube(originalVertices2, vertices2, distance, faces2, 240, bmptemp, colors2);
                bmptemp = drawCube(originalVertices1, vertices1, distance, faces1, 240, bmptemp, colors1);
                pictureBox1.Image = bmptemp;
            }
        }

        private void form_MouseWheel(object sender, MouseEventArgs e)
        { 
            const float scale_per_delta = 0.1f / 120;

            distance = distance + e.Delta * scale_per_delta * distance;
            if (distance < 0) distance = 0;
            clearZBuffer();
            bmptemp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmptemp = drawCube(originalVertices2, vertices2, distance, faces2, 240, bmptemp, colors2);
            bmptemp = drawCube(originalVertices1, vertices1, distance, faces1, 240, bmptemp, colors1);
            pictureBox1.Image = bmptemp;
        }

        public void clearZBuffer()
        {
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    zBuffer[i, j] = -10000;
                }
            }
        }

        public Bitmap drawCube(Point4D[] orgV, Point4D[] newV, float dist, face[] faces, float angle, Bitmap bmp, Color[] colors)
        {
            Bitmap bmpt = bmp;
            //graphics.Clear(Color.Black);
            float fi = angle; //120
            float d = dist;
            float sx = pictureBox1.Width;
            float sy = pictureBox1.Height;

            Matrix3D Ry = new Matrix3D(Math.Cos(alfa), 0, -Math.Sin(alfa), 0,
                            0, 1, 0, 0,
                            Math.Sin(alfa), 0, Math.Cos(alfa), 0,
                            0, 0, 0, 1);

            Matrix3D Rx = new Matrix3D(1, 0, 0, 0,
                            0, Math.Cos(beta), Math.Sin(beta), 0,
                            0, -Math.Sin(beta), Math.Cos(beta), 0,
                            0, 0, 0, 1);

            Matrix3D T = new Matrix3D(1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, d, 1); //model, transformation matrix

            Matrix3D P = new Matrix3D(sx / (2 * Math.Tan(fi / 2)), 0, 0, 0,
                                        0, -sx / (2 * Math.Tan(fi / 2)), 0, 0,
                                        -sx / 2, -sy / 2, 0, -1,
                                        0, 0, -1, 0); //perspective projection matrix

            for (int i = 0; i < 8; i++)
            {
                Point4D p = orgV[i];

                Point4D s = Point4D.Multiply(p, Ry);
                Point4D q = Point4D.Multiply(s, Rx);
                Point4D r = Point4D.Multiply(q, T);
                Point4D w = Point4D.Multiply(r, P);

                w.X /= w.W;
                w.Y /= w.W;
                w.Z /= w.W;
                w.W /= w.W;

                newV[i] = w;
            }

            //for (int i = 0; i < 4; i++)
            //{
            //    graphics.DrawLine(pen, (float)(newV[i].X), (float)(newV[i].Y), (float)(newV[(i + 1) % 4].X), (float)(newV[(i + 1) % 4].Y));
            //    graphics.DrawLine(pen, (float)(newV[i + 4].X), (float)(newV[i + 4].Y), (float)(newV[(i + 1) % 4 + 4].X), (float)(newV[(i + 1) % 4 + 4].Y));
            //    graphics.DrawLine(pen, (float)(newV[i].X), (float)(newV[i].Y), (float)(newV[(i + 1) % 4 + 4].X), (float)(newV[(i + 1) % 4 + 4].Y));
            //}

            faces[0] = new face(newV[0], newV[1], newV[2], newV[3], new Point4D(0, -1, 0, 1), colors[0]);
            faces[1] = new face(newV[0], newV[1], newV[6], newV[5], new Point4D(0, 0, 1, 1), colors[1]);
            faces[2] = new face(newV[1], newV[2], newV[7], newV[6], new Point4D(1, 0, 0, 1), colors[2]);
            faces[3] = new face(newV[2], newV[3], newV[4], newV[7], new Point4D(0, 0, -1, 1), colors[3]);
            faces[4] = new face(newV[0], newV[3], newV[4], newV[5], new Point4D(-1, 0, 0, 1), colors[4]);
            faces[5] = new face(newV[4], newV[5], newV[6], newV[7], new Point4D(0, 1, 0, 1), colors[5]);

            for (int i = 0; i < 6; i++)
            {             
                bmpt = fillPolygon(faces[i].vertices.ToList(), faces[i].color, bmpt);      
            }
            return bmpt;
        }

        public Point[] to2D(Point4D[] p)
        {
            int size = p.Length;
            Point[] points = new Point[size];
            for(int i = 0; i < size; i++)
            {
                points[i] = new Point((int)p[i].X, (int)p[i].Y);
            }

            return points;
        }

        public List<ETentry> getEgdeTable(List<Point4D> polygon)
        {
            List<ETentry> edgeTable = new List<ETentry>();
            Point4D temp = polygon.Last();

            foreach (var v in polygon)
            {
                ETentry et = new ETentry();
                et.ymin = (int)Math.Min(v.Y, temp.Y);
                et.ymax = (int)Math.Max(v.Y, temp.Y);
                if (v.Y < temp.Y)
                    et.xmin = (float)v.X;
                else
                    et.xmin = (float)temp.X;

                if (v.Y < temp.Y)
                    et.zmin = (float)v.Z;
                else
                    et.zmin = (float)temp.Z;
                int dy = (int)(v.Y - temp.Y);
                int dx = (int)(v.X - temp.X);
                int dz = (int)(v.Z - temp.Z);
                //if (dy == 0) et.oneoverm = 0;
                if (dy == 0) continue;
                et.oneoverm = (float)dx / (float)dy; 
                et.dzOVERdy = (float)dz / (float)dy;
                edgeTable.Add(et);

                temp = v;
            }

            edgeTable.Sort((p, q) => p.ymin.CompareTo(q.ymin));

            return edgeTable;
        }

        public Bitmap fillPolygon(List<Point4D> polygon, Color color, Bitmap bmp)
        {
            Bitmap bmpt = bmp;
            List<ETentry> edgeTable = getEgdeTable(polygon);
            ETentry ETmin = edgeTable[0];
            int y = ETmin.ymin;
            List<ETentry> activeEdgeTable = new List<ETentry>();
            SolidBrush br = new SolidBrush(color);
            while (edgeTable.Count != 0 || activeEdgeTable.Count != 0)
            {
                List<ETentry> toRemove = new List<ETentry>();
                foreach (var et in edgeTable)
                {
                    if (et.ymin == y)
                    {
                        activeEdgeTable.Add(et);
                        toRemove.Add(et);
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (var et in toRemove)
                {
                    edgeTable.Remove(et);
                }
                toRemove.Clear();
                activeEdgeTable.Sort((p, q) => p.xmin.CompareTo(q.xmin));

                for (int n = 0; n < activeEdgeTable.Count; n += 2)
                {
                    if (n + 1 < activeEdgeTable.Count)
                    {
                        float tempZ = activeEdgeTable[n].zmin;
                        float diff = (activeEdgeTable[n+1].zmin - activeEdgeTable[n].zmin)/(activeEdgeTable[n+1].xmin - activeEdgeTable[n].xmin);
                        for (int a = (int)activeEdgeTable[n].xmin; a <= activeEdgeTable[n + 1].xmin; a++)
                        {
                            if(a < pictureBox1.Width && y < pictureBox1.Height && a >= 0 && y >= 0)
                                if (tempZ >= zBuffer[a, y])
                                {
                                    bmpt.SetPixel(a, y, color);
                                    zBuffer[a, y] = tempZ;
                                }
                            tempZ += diff ;
                        }
                    }
                }

                ++y;

                foreach (var e in activeEdgeTable.ToList())
                {
                    if (e.ymax == y)
                        activeEdgeTable.Remove(e);
                }

                foreach (var e in activeEdgeTable.ToList())
                {
                    e.xmin += e.oneoverm; //dx/dy
                    e.zmin += e.dzOVERdy;
                }
            }
            return bmpt;
        }
    }
}

/*
 
     public partial class Form1 : Form
    {
        public Bitmap bmp;
        public Bitmap pixel;
        public Bitmap bmp2;
        public Graphics graphics;
        public Pen pen;
        public int midPointX;
        public int midPointY;
        public int length = 50;
        public int height = 25;
        public int difference = 50;
        public Point4D[] originalVeritces1;
        public Point4D[] vertices1;
        public face[] faces1;
        //public Point4D[] originalVertices2;
        //public Point4D[] veritces2;
        //public face[] faces2;
        public float alfa = 0;
        public Point3D cPos, cTarget;
        public Vector3D cUp;

        public class face
        {
            Point4D[] vertices;
            Point4D normal;
            Point4D[] triangle1;
            Point4D[] triangle2;

            public face(Point4D v1, Point4D v2, Point4D v3, Point4D v4, Point4D n)
            {
                vertices = new Point4D[4];
                vertices[0] = v1;
                vertices[1] = v2;
                vertices[2] = v3;
                vertices[3] = v4;
                normal = n;
                triangle1 = new Point4D[3] { vertices[0], vertices[1], vertices[2] };
                triangle2 = new Point4D[3] { vertices[0], vertices[2], vertices[3] };
            }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = null;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp2 = new Bitmap(pictureBox1.Width * 2, pictureBox1.Height * 2);
            pixel = new Bitmap(1, 1);
            pictureBox1.Image = bmp;
            graphics = pictureBox1.CreateGraphics();
            pen = new Pen(Color.Black, 1);
            midPointX = pictureBox1.Width / 2;
            midPointY = pictureBox1.Height / 2;
            originalVeritces1 = new Point4D[8];
            vertices1 = new Point4D[8];
            originalVeritces1[0] = new Point4D(-length / 2, -height, -length / 2, 1);
            originalVeritces1[1] = new Point4D(length / 2, -height, -length / 2, 1);
            originalVeritces1[2] = new Point4D(length / 2, -height, length / 2, 1);
            originalVeritces1[3] = new Point4D(-length / 2, -height, length / 2, 1);
            originalVeritces1[4] = new Point4D(-length / 2, height, length / 2, 1);
            originalVeritces1[5] = new Point4D(-length / 2, height, -length / 2, 1);
            originalVeritces1[6] = new Point4D(length / 2, height, -length / 2, 1);
            originalVeritces1[7] = new Point4D(length / 2, height, length / 2, 1);
            faces1 = new face[6];
            faces1[0] = new face(originalVeritces1[0], originalVeritces1[1], originalVeritces1[2], originalVeritces1[3], new Point4D(0, -1, 0, 1));
            faces1[1] = new face(originalVeritces1[0], originalVeritces1[1], originalVeritces1[6], originalVeritces1[5], new Point4D(0, 0, 1, 1));
            faces1[2] = new face(originalVeritces1[1], originalVeritces1[2], originalVeritces1[7], originalVeritces1[6], new Point4D(1, 0, 0, 1));
            faces1[3] = new face(originalVeritces1[2], originalVeritces1[3], originalVeritces1[4], originalVeritces1[7], new Point4D(0, 0, -1, 1));
            faces1[4] = new face(originalVeritces1[0], originalVeritces1[3], originalVeritces1[4], originalVeritces1[5], new Point4D(-1, 0, 0, 1));
            faces1[5] = new face(originalVeritces1[4], originalVeritces1[5], originalVeritces1[6], originalVeritces1[7], new Point4D(0, 1, 0, 1));

            //originalVertices2 = new Point4D[8];
            //veritces2 = new Point4D[8];
            //originalVertices2[0] = new Point4D(-length / 2 + difference, -height + difference, -length / 2 + difference, 1);
            //originalVertices2[1] = new Point4D(length / 2 + difference, -height + difference, -length / 2 + difference, 1);
            //originalVertices2[2] = new Point4D(length / 2 + difference, -height + difference, length / 2 + difference, 1);
            //originalVertices2[3] = new Point4D(-length / 2 + difference, -height + difference, length / 2 + difference, 1);
            //originalVertices2[4] = new Point4D(-length / 2 + difference, height + difference, length / 2 + difference, 1);
            //originalVertices2[5] = new Point4D(-length / 2 + difference, height + difference, -length / 2 + difference, 1);
            //originalVertices2[6] = new Point4D(length / 2 + difference, height + difference, -length / 2 + difference, 1);
            //originalVertices2[7] = new Point4D(length / 2 + difference, height + difference, length / 2 + difference, 1);
            //faces2 = new face[6];
            //faces2[0] = new face(originalVertices2[0], originalVertices2[1], originalVertices2[2], originalVertices2[3], new Point4D(0, -1, 0, 0));
            //faces2[1] = new face(originalVertices2[0], originalVertices2[1], originalVertices2[6], originalVertices2[5], new Point4D(0, 0, 1, 0));
            //faces2[2] = new face(originalVertices2[1], originalVertices2[2], originalVertices2[7], originalVertices2[6], new Point4D(1, 0, 0, 0));
            //faces2[3] = new face(originalVertices2[2], originalVertices2[3], originalVertices2[4], originalVertices2[7], new Point4D(0, 0, -1, 0));
            //faces2[4] = new face(originalVertices2[0], originalVertices2[3], originalVertices2[4], originalVertices2[5], new Point4D(-1, 0, 0, 0));
            //faces2[5] = new face(originalVertices2[4], originalVertices2[5], originalVertices2[6], originalVertices2[7], new Point4D(0, 1, 0, 0));

            cPos = new Point3D(0, 0,   10);
            cTarget = new Point3D(0, 0, 0);
            //cUp = new Vector3D(20, 30, 40);

            graphics.Clear(Color.White);
        }

        private void selectPointPolygon_Click(object sender, EventArgs e)
        {
            alfa += 30;
            graphics.Clear(Color.White);
            Matrix3D Ry = new Matrix3D(Math.Cos(alfa), 0, -Math.Sin(alfa), 0, 0, 1, 0, 0, Math.Sin(alfa), 0, Math.Cos(alfa), 0, 0, 0, 0, 1);
            for (int i = 0; i < 8; i++)
            {
                Point4D p = vertices1[i];
                Point4D q = Point4D.Multiply(p, Ry);
                vertices1[i] = q;
            }

            for (int i = 0; i < 4; i++)
            {
                graphics.DrawLine(pen, (float)(vertices1[i].X), (float)(vertices1[i].Y), (float)(vertices1[(i + 1) % 4].X), (float)(vertices1[(i + 1) % 4].Y));
            }

            for (int i = 0; i < 4; i++)
            {
                graphics.DrawLine(pen, (float)(vertices1[i].X), (float)(vertices1[i].Y), (float)(vertices1[4].X), (float)(vertices1[4].Y));
            }
        }

        private void drawCoordButton_Click(object sender, EventArgs e)
        {
            drawCube(originalVeritces1, vertices1);
        }


        public void drawCube(Point4D[] orgV, Point4D[] newV)
        {
            graphics.Clear(Color.White);
            float fi = 120;
            float degrees = 90;
            float sx = pictureBox1.Width;
            float sy = pictureBox1.Width;
            float d = sx / 2 * (float)(1 / Math.Tan(degrees / 2));
            float cx = pictureBox1.Width / 2;
            float cy = pictureBox1.Height / 2;
            Matrix3D Ry = new Matrix3D(Math.Cos(alfa), 0, -Math.Sin(alfa), 0,
                            0, 1, 0, 0,
                            Math.Sin(alfa), 0, Math.Cos(alfa), 0,
                            0, 0, 0, 1);

            Matrix3D T = new Matrix3D(1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 150, 1);

            Matrix3D M = new Matrix3D(d, 0, 0, 0,
                                        0, -d, 0, 0,
                                        cx, cy, 0, 1,
                                        0, 0, 0, 0);

            Matrix3D P = new Matrix3D(sx / (2 * Math.Tan(fi / 2)), 0, 0, 0,
                                        0, -sx / (2 * fi * Math.Tan(fi / 2)), 0, 0,
                                        -sx / 2, -sy / 2, 0, -1,
                                        0, 0, -1, 0);

            for (int i = 0; i < 8; i++)
            {
                Point4D p = orgV[i];

                Point4D s = Point4D.Multiply(p, Ry);
                Point4D q = Point4D.Multiply(s, T);
                Point4D r = Point4D.Multiply(q, M);
                //Point4D w = Point4D.Multiply(r, P);

                newV[i] = r;
            }

            for (int i = 0; i < 4; i++)
            {
                graphics.DrawLine(pen, (float)(newV[i].X / newV[i].W), (float)(newV[i].Y / newV[i].W), (float)(newV[(i + 1) % 4].X / newV[(i + 1) % 4].W), (float)(newV[(i + 1) % 4].Y / newV[(i + 1) % 4].W));
                graphics.DrawLine(pen, (float)(newV[i + 4].X / newV[i + 4].W), (float)(newV[i + 4].Y / newV[i + 4].W), (float)(newV[(i + 1) % 4 + 4].X / newV[(i + 1) % 4 + 4].W), (float)(newV[(i + 1) % 4 + 4].Y / newV[(i + 1) % 4 + 4].W));
                graphics.DrawLine(pen, (float)(newV[i].X / newV[i].W), (float)(newV[i].Y / newV[i].W), (float)(newV[(i + 1) % 4 + 4].X / newV[(i + 1) % 4 + 4].W), (float)(newV[(i + 1) % 4 + 4].Y / newV[(i + 1) % 4 + 4].W));
            }
            alfa += 30;
        }
    }
    
 * */


/*
 
       public partial class Form1 : Form
    {
        public Mesh mesh;
        public Camera mera;
        public Device device;

        public class Camera
        {
            public Vector3D Position { get; set; }
            public Vector3D Target { get; set; }
        }
        public class Mesh
        {
            public string Name { get; set; }
            public Vector3D[] Vertices { get; private set; }
            public Vector3D Position { get; set; }
            public Vector3D Rotation { get; set; }

            public Mesh(string name, int verticesCount)
            {
                Vertices = new Vector3D[verticesCount];
                Name = name;
            }
        }

        public class Device
        {
            private byte[] backBuffer;
            private WriteableBitmap bmp;

            public Device(WriteableBitmap bmp)
            {
                this.bmp = bmp;
                // the back buffer size is equal to the number of pixels to draw
                // on screen (width*height) * 4 (R,G,B & Alpha values). 
                backBuffer = new byte[bmp.PixelWidth * bmp.PixelHeight * 4];
            }

            // This method is called to clear the back buffer with a specific color
            public void Clear(byte r, byte g, byte b, byte a)
            {
                for (var index = 0; index < backBuffer.Length; index += 4)
                {
                    // BGRA is used by Windows instead by RGBA in HTML5
                    backBuffer[index] = b;
                    backBuffer[index + 1] = g;
                    backBuffer[index + 2] = r;
                    backBuffer[index + 3] = a;
                }
            }

            // Once everything is ready, we can flush the back buffer
            // into the front buffer. 
            //public void Present()
            //{
            //    using (Stream stream = bmp.PixelBuffer.AsStream())
            //    {
            //        // writing our byte[] back buffer into our WriteableBitmap stream
            //        stream.Write(backBuffer, 0, backBuffer.Length);
            //    }
            //    // request a redraw of the entire bitmap
            //    bmp.InvalidateProperty;
            //}

            // Called to put a pixel on screen at a specific X,Y coordinates
            public void PutPixel(int x, int y, System.Drawing.Color color)
            {
                // As we have a 1-D Array for our back buffer
                // we need to know the equivalent cell in 1-D based
                // on the 2D coordinates on screen
                var index = (x + y * bmp.PixelWidth) * 4;

                backBuffer[index] = (byte)(color.B * 255);
                backBuffer[index + 1] = (byte)(color.G * 255);
                backBuffer[index + 2] = (byte)(color.R * 255);
                backBuffer[index + 3] = (byte)(color.A * 255);
            }

            // Project takes some 3D coordinates and transform them
            // in 2D coordinates using the transformation matrix
            public Point Project(Vector3D coord, Matrix3D transMat)
            {
                // transforming the coordinates
                var point = Vector3D.Multiply(coord, transMat);
                // The transformed coordinates will be based on coordinate system
                // starting on the center of the screen. But drawing on screen normally starts
                // from top left. We then need to transform them again to have x:0, y:0 on top left.
                var x = point.X * bmp.PixelWidth + bmp.PixelWidth / 2.0f;
                var y = -point.Y * bmp.PixelHeight + bmp.PixelHeight / 2.0f;
                return (new Point((int)x, (int)y));
            }

            // DrawPoint calls PutPixel but does the clipping operation before
            public void DrawPoint(Point point)
            {
                // Clipping what's visible on screen
                if (point.X >= 0 && point.Y >= 0 && point.X < bmp.PixelWidth && point.Y < bmp.PixelHeight)
                {
                    // Drawing a yellow point
                    PutPixel((int)point.X, (int)point.Y, System.Drawing.Color.Black);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = null;

            mesh = new Mesh("Cube", 8);
            mesh.Vertices[0] = new Vector3D(-1, 1, 1);
            mesh.Vertices[1] = new Vector3D(1, 1, 1);
            mesh.Vertices[2] = new Vector3D(-1, -1, 1);
            mesh.Vertices[3] = new Vector3D(-1, -1, -1);
            mesh.Vertices[4] = new Vector3D(-1, 1, -1);
            mesh.Vertices[5] = new Vector3D(1, 1, -1);
            mesh.Vertices[6] = new Vector3D(1, -1, 1);
            mesh.Vertices[7] = new Vector3D(1, -1, -1);

            Camera mera = new Camera();

            WriteableBitmap bmp = new WriteableBitmap((int)pictureBox1.Width,
                (int)pictureBox1.Height,
                640,
                480,
                PixelFormats.Bgr32,
                null);

            device = new Device(bmp);

            mera.Position = new Vector3D(0, 0, 10.0f);
            mera.Target = new Vector3D(0,0,0);

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            device.Clear(0, 0, 0, 255);

            // rotating slightly the cube during each frame rendered
            mesh.Rotation = new Vector3D(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);

            // Doing the various matrix operations
            //device.Render(mera, mesh);
            // Flushing the back buffer into the front buffer


        }

        private void selectPointPolygon_Click(object sender, EventArgs e)
        { }
        private void drawCoordButton_Click(object sender, EventArgs e)
        { }
    }

*/

/* point3D origin = new point3D(center, center, center);
           point3D axisX = new point3D(1, 0, 0);
           point3D axisY = new point3D(0, 1, 0);
           point3D axisZ = new point3D(0, 0, 1);

           point3D axisXtemp = new point3D(0, 0, 0);
           point3D axisYtemp = new point3D(0, 0, 0);
           point3D axisZtemp = new point3D(0, 0, 0);
           axisXtemp.x = origin.x + (axisX.x * edge);
           axisXtemp.y = origin.y + (axisX.y * edge);
           axisXtemp.z = origin.z + (axisX.z * edge);

           axisYtemp.x = origin.x + (axisY.x * edge);
           axisYtemp.y = origin.y + (axisY.y * edge);
           axisYtemp.z = origin.z + (axisY.z * edge);

           axisZtemp.x = origin.x + (axisZ.x * edge);
           axisZtemp.y = origin.y + (axisZ.y * edge);
           axisZtemp.z = origin.z + (axisZ.z * edge);

           Point mp = to2D(origin);
           Point pX = to2D(axisXtemp);
           Point pY = to2D(axisYtemp);
           Point pZ = to2D(axisZtemp);

           graphics.DrawLine(pen, mp.X, mp.Y, pX.X, pX.Y);
           graphics.DrawLine(pen, mp.X, mp.Y, pY.X, pY.Y);
           graphics.DrawLine(pen, mp.X, mp.Y, pZ.X, pZ.Y); */


//public void drawCube(Point4D[] orgV, Point4D[] newV)
//{
//    graphics.Clear(Color.White);
//    float degrees = 90;
//    float w = pictureBox1.Width;
//    float d = w / 2 * (float)(1 / Math.Tan(degrees / 2));
//    float cx = pictureBox1.Width / 2;
//    float cy = pictureBox1.Height / 2;
//    Matrix3D T = new Matrix3D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 150, 1);
//    Matrix3D M = new Matrix3D(d, 0, 0, 0, 0, -d, 0, 0, cx, cy, 0, 1, 0, 0, 0, 0);
//    Matrix3D Ry = new Matrix3D(Math.Cos(alfa), 0, -Math.Sin(alfa), 0, 0, 1, 0, 0, Math.Sin(alfa), 0, Math.Cos(alfa), 0, 0, 0, 0, 1);

//    for (int i = 0; i < 8; i++)
//    {
//        Point4D p = orgV[i];

//        Point4D s = Point4D.Multiply(p, Ry);
//        Point4D q = Point4D.Multiply(s, T);
//        Point4D r = Point4D.Multiply(q, M);

//        newV[i] = r;
//    }

//    for (int i = 0; i < 4; i++)
//    {
//        graphics.DrawLine(pen, (float)(newV[i].X / newV[i].W), (float)(newV[i].Y / newV[i].W), (float)(newV[(i + 1) % 4].X / newV[(i + 1) % 4].W), (float)(newV[(i + 1) % 4].Y / newV[(i + 1) % 4].W));
//        graphics.DrawLine(pen, (float)(newV[i + 4].X / newV[i + 4].W), (float)(newV[i + 4].Y / newV[i + 4].W), (float)(newV[(i + 1) % 4 + 4].X / newV[(i + 1) % 4 + 4].W), (float)(newV[(i + 1) % 4 + 4].Y / newV[(i + 1) % 4 + 4].W));
//        graphics.DrawLine(pen, (float)(newV[i].X / newV[i].W), (float)(newV[i].Y / newV[i].W), (float)(newV[(i + 1) % 4 + 4].X / newV[(i + 1) % 4 + 4].W), (float)(newV[(i + 1) % 4 + 4].Y / newV[(i + 1) % 4 + 4].W));
//    }
//    alfa += 30;
//}