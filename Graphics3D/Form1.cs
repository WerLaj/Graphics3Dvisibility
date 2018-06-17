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

            colors1 = new Color[6] { Color.Peru, Color.Sienna, Color.Gray, Color.Moccasin, Color.Tan, Color.Maroon };
            colors2 = new Color[6] { Color.Moccasin, Color.Tan, Color.Maroon, Color.Peru, Color.Sienna, Color.Gray};
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
                float dy = (float)(v.Y - temp.Y);
                float dx = (float)(v.X - temp.X);
                float dz = (float)(v.Z - temp.Z);
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