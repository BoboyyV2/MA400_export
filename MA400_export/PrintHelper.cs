using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace MA400_export
{
    /**
     * <summary>
     * Gere l'impression du document (pièce + liste des goujons)
     * sans afficher de Form à l'écran.
     *
     * Utilisation depuis Form1 :
     *     var helper = new PrintHelper(gc, fs.Studs, _Zoom, Origin_Offset);
     *     helper.ShowPreview(this);          // aperçu avant impression
     *     // OU
     *     helper.Print();                    // impression directe
     * </summary>
    */
    public class PrintHelper
    {
        //vue principale
        private readonly GraphicsContainer _gc;
        private readonly IEnumerable<Stud> _studs;
        private readonly float _zoom;
        private readonly PointF _originOffset;

        //dimensions du bitmap de la pièce
        // On prend la taille "native" du workzone (750 mm + offset de 50 px)
        private const int PIECE_BITMAP_W = 800;
        private const int PIECE_BITMAP_H = 600;

        private PrintDocument _printDocument;

        public PrintHelper(GraphicsContainer gc,
                           IEnumerable<Stud> studs,
                           float zoom,
                           PointF originOffset)
        {
            _gc           = gc;
            _studs        = studs;
            _zoom         = zoom;
            _originOffset = originOffset;

            _printDocument = new PrintDocument();
            _printDocument.DefaultPageSettings.Landscape = false;
            _printDocument.PrintPage += OnPrintPage;
        }

        

        /*
         * <summary>Ouvre la boîte d'aperçu avant impression.</summary>
        */
        public void ShowPreview(IWin32Window owner = null)
        {
            using (var dlg = new PrintPreviewDialog())
            {
                dlg.Document  = _printDocument;
                dlg.WindowState = FormWindowState.Maximized;
                dlg.ShowInTaskbar = false;
                dlg.ShowDialog(owner);
            }
        }

        /*
         * <summary>Imprime directement (avec boîte de dialogue imprimante).</summary>
         */
        public void Print(IWin32Window owner = null)
        {
            using (var dlg = new PrintDialog())
            {
                dlg.Document = _printDocument;
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    _printDocument.Print();
            }
        }

        // ___________________________RENDU___________________________  

        /* 
         * <summary>
         * Crée un Bitmap de la pièce en réutilisant exactement la meme
         * logique que le Paint() du panneau principal, mais dans un
         * Graphics off-screen => aucun Form n'est affiché.
         * </summary>
         */
        private Bitmap RenderPieceBitmap(int width, int height)
        {
            var bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                //sauvegarde le Graphics GC into le remplace
                //temporairement pour dessiner dans le bitmap.
                Graphics savedGraphics = _gc.graphics;
                _gc.graphics = g;

                //paint la pièce 
                _gc.Paint(_studs,
                          Enumerable.Empty<Stud>(), // pas de sélection à l'impression
                          _originOffset);

                //reset 
                _gc.graphics = savedGraphics;
            }

            return bmp;
        }

        // Handler
        // tout le dessin de la page se fait ici

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g     = e.Graphics;
            Rectangle page = e.MarginBounds; // zone imprimable 

            //mise en page
            int pieceAreaHeight = (int)(page.Height * 0.55f);  
            int listAreaTop     = page.Top + pieceAreaHeight + 10;
            int listAreaHeight  = page.Height - pieceAreaHeight - 20;

            Rectangle pieceRect = new Rectangle(page.Left, page.Top,
                                                page.Width, pieceAreaHeight);
            Rectangle listRect  = new Rectangle(page.Left, listAreaTop,
                                                page.Width, listAreaHeight);

            //dessin
            DrawPieceArea(g, pieceRect);

            //separateur entre les 2 zones
            g.DrawLine(Pens.Black,
                       page.Left,  listAreaTop - 5,
                       page.Right, listAreaTop - 5);

            //liste des goujons
            DrawStudList(g, listRect);

            //une page
            e.HasMorePages = false;
        }

        //zone de la pièce
        

        private void DrawPieceArea(Graphics g, Rectangle area)
        {
            //titre
            using (Font titleFont = new Font("Arial", 12, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                g.DrawString("Pièce", titleFont, brush, area.Left, area.Top);
            }

            int titleHeight = 22;
            Rectangle imageRect = new Rectangle(
                area.Left, area.Top + titleHeight,
                area.Width, area.Height - titleHeight);

            //rendu off-screen
            using (Bitmap pieceBmp = RenderPieceBitmap(PIECE_BITMAP_W, PIECE_BITMAP_H))
            {
                //adapte le bitmap à la zone disponible en conservant le ratio
                Rectangle destRect = FitKeepAspect(pieceBmp.Size, imageRect);

                g.FillRectangle(Brushes.Black, destRect);
                g.DrawImage(pieceBmp, destRect);

                g.DrawRectangle(Pens.DarkGray, destRect);
            }
        }

        //zone liste des goujons

        private void DrawStudList(Graphics g, Rectangle area)
        {
            var studList = _studs.ToList();

            using (Font titleFont  = new Font("Arial", 11, FontStyle.Bold))
            using (Font headerFont = new Font("Arial",  9, FontStyle.Bold | FontStyle.Underline))
            using (Font cellFont   = new Font("Arial",  8))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                float y = area.Top;

                //titre de la section
                g.DrawString($"Goujons ({studList.Count})", titleFont, brush, area.Left, y);
                y += 20;

                if (studList.Count == 0)
                {
                    g.DrawString("Aucun goujon.", cellFont, brush, area.Left, y);
                    return;
                }

                //header
                //   N°  |  X (mm)  |  Y (mm)  |  Ø (mm)
                float colN = area.Left;
                float colX = area.Left + 40;
                float colY = area.Left + 130;
                float colD = area.Left + 220;

                g.DrawString("#",        headerFont, brush, colN, y);
                g.DrawString("X (mm)",   headerFont, brush, colX, y);
                g.DrawString("Y (mm)",   headerFont, brush, colY, y);
                g.DrawString("Ø (mm)",   headerFont, brush, colD, y);
                y += 16;

                //lignes de données
                //calcule combien de lignes tiennent dans la hauteur restante
                float rowH    = 14f;
                int   maxRows = (int)((area.Bottom - y) / rowH);

                if(maxRows < 1) maxRows = 1;

                //colonnes multiples si beaucoup de goujons
                int colCount = (int)Math.Ceiling((double)studList.Count / maxRows);
                if(colCount < 1) colCount = 1;

                float colGroupWidth = area.Width / colCount;

                for (int idx = 0; idx < studList.Count; idx++)
                {
                    int colGroup = idx / maxRows;
                    int rowInCol = idx % maxRows;
                    float xBase  = area.Left + colGroup * colGroupWidth;
                    float yRow   = y + rowInCol * rowH;

                    var stud = studList[idx];
                    double xMm = stud.circle.Center.X;
                    double yMm = stud.circle.Center.Y;
                    double dMm = stud.circle.Radius * 2;

                    
                    if (rowInCol % 2 == 0)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)),
                            xBase, yRow, colGroupWidth - 2, rowH);
                    }

                    g.DrawString((idx + 1).ToString(), cellFont, brush,
                                 xBase + (colN - area.Left), yRow);
                    g.DrawString(xMm.ToString("0.00"), cellFont, brush,
                                 xBase + (colX - area.Left), yRow);
                    g.DrawString(yMm.ToString("0.00"), cellFont, brush,
                                 xBase + (colY - area.Left), yRow);
                    g.DrawString(dMm.ToString("0.00"), cellFont, brush,
                                 xBase + (colD - area.Left), yRow);
                }
            }
        }

        
        //utilitaire : ajustement avec conservation du ratio

        private static Rectangle FitKeepAspect(Size source, Rectangle dest)
        {
            if (source.Width == 0 || source.Height == 0)
                return dest;

            float scaleW = (float)dest.Width  / source.Width;
            float scaleH = (float)dest.Height / source.Height;
            float scale  = Math.Min(scaleW, scaleH);

            int w = (int)(source.Width  * scale);
            int h = (int)(source.Height * scale);
            int x = dest.Left + (dest.Width  - w) / 2;
            int y = dest.Top  + (dest.Height - h) / 2;

            return new Rectangle(x, y, w, h);
        }

    }
}
