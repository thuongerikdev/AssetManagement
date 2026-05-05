/**
 * Render Mermaid class diagram & ER diagram → PNG files
 * then embed into a Word .docx
 */
import puppeteer from 'puppeteer';
import { writeFileSync, readFileSync } from 'fs';
import {
  Document, Packer, Paragraph, Table, TableRow, TableCell,
  TextRun, HeadingLevel, AlignmentType, WidthType,
  BorderStyle, ShadingType, ImageRun, TableLayoutType,
} from 'docx';

// ─── Mermaid diagram definitions ─────────────────────────────────────────────

const CLASS_DIAGRAM = `
classDiagram
  direction TB

  class TaiSan {
    +int Id
    +string MaTaiSan
    +string TenTaiSan
    +TrangThaiTaiSan TrangThai
    +string SoSeri
    +DateTime NgayMua
    +decimal NguyenGia
    +decimal GiaTriConLai
    +decimal KhauHaoLuyKe
    +decimal KhauHaoHangThang
    +PhuongPhapKhauHao PhuongPhapKhauHao
    +int ThoiGianKhauHao
    +int DanhMucId
    +int PhongBanId
    +int LoId
    +string MaTaiKhoan
  }

  class ChungTu {
    +int Id
    +string MaChungTu
    +DateTime NgayLap
    +LoaiChungTu LoaiChungTu
    +string MoTa
    +decimal TongTien
    +string TrangThai
    +int NguoiLapId
  }

  class ChiTietChungTu {
    +int Id
    +int ChungTuId
    +string TaiKhoanNo
    +string TaiKhoanCo
    +decimal SoTien
    +string MoTa
    +int TaiSanId
  }

  class LichSuKhauHao {
    +int Id
    +int TaiSanId
    +int ChungTuId
    +string KyKhauHao
    +decimal SoTien
    +decimal LuyKeSauKhauHao
    +decimal ConLaiSauKhauHao
    +DateTime NgayTao
  }

  class DieuChuyenTaiSan {
    +int Id
    +int TaiSanId
    +LoaiDieuChuyen LoaiDieuChuyen
    +DateTime NgayThucHien
    +int TuPhongBanId
    +int DenPhongBanId
    +int TuNguoiDungId
    +int DenNguoiDungId
    +string TrangThai
  }

  class BaoTriTaiSan {
    +int Id
    +int TaiSanId
    +string LoaiBaoTri
    +DateTime NgayThucHien
    +string MoTa
    +bool CoChiPhi
    +decimal ChiPhi
    +TrangThaiBaoTri TrangThai
  }

  class ThanhLyTaiSan {
    +int Id
    +int TaiSanId
    +DateTime NgayThanhLy
    +decimal NguyenGia
    +decimal KhauHaoLuyKe
    +decimal GiaTriConLai
    +decimal GiaTriThanhLy
    +decimal LaiLo
    +string LyDo
    +TrangThaiThanhLy TrangThai
  }

  class TaiSanDinhKem {
    +int Id
    +int TaiSanId
    +string TenFile
    +string LoaiFile
    +string DuongDan
    +long KichThuoc
    +DateTime NgayTai
    +string MoTa
  }

  class DanhMucTaiSan {
    +int Id
    +string MaDanhMuc
    +string TenDanhMuc
    +string TienTo
    +int ThoiGianKhauHao
    +string MaTaiKhoan
  }

  class PhongBan {
    +int Id
    +string MaPhongBan
    +string TenPhongBan
  }

  class TaiKhoanKeToan {
    +int Id
    +string MaTaiKhoan
    +string TenTaiKhoan
    +string LoaiTaiKhoan
    +string MaTaiKhoanCha
  }

  class LoTaiSan {
    +int Id
    +string MaLo
    +int SoLuong
    +decimal TongGiaTri
    +DateTime NgayTao
  }

  TaiSan "n" --> "1" DanhMucTaiSan : DanhMucId
  TaiSan "n" --> "1" PhongBan : PhongBanId
  TaiSan "n" --> "1" LoTaiSan : LoId
  TaiSan "n" --> "1" TaiKhoanKeToan : MaTaiKhoan
  TaiSan "1" --> "n" ChiTietChungTu : Id
  TaiSan "1" --> "n" LichSuKhauHao : Id
  TaiSan "1" --> "n" DieuChuyenTaiSan : Id
  TaiSan "1" --> "n" BaoTriTaiSan : Id
  TaiSan "1" --> "1" ThanhLyTaiSan : Id
  TaiSan "1" --> "n" TaiSanDinhKem : Id
  ChungTu "1" --> "n" ChiTietChungTu : Id
  ChungTu "1" --> "n" LichSuKhauHao : Id
  DanhMucTaiSan "n" --> "1" TaiKhoanKeToan : MaTaiKhoan
  TaiKhoanKeToan "1" --> "n" TaiKhoanKeToan : MaTaiKhoanCha
  DieuChuyenTaiSan "n" --> "1" PhongBan : TuPhongBanId
  DieuChuyenTaiSan "n" --> "1" PhongBan : DenPhongBanId
`;

const ER_DIAGRAM = `
erDiagram
  tai_san {
    int id PK
    varchar ma_tai_san UK
    varchar ten_tai_san
    varchar trang_thai
    int danh_muc_id FK
    int phong_ban_id FK
    int lo_id FK
    varchar ma_tai_khoan FK
    decimal nguyen_gia
    decimal gia_tri_con_lai
    decimal khau_hao_luy_ke
    int thoi_gian_khau_hao
    int nguoi_dung_id
  }

  danh_muc_tai_san {
    int id PK
    varchar ma_danh_muc UK
    varchar ten_danh_muc
    varchar tien_to
    int thoi_gian_khau_hao
    varchar ma_tai_khoan FK
  }

  phong_ban {
    int id PK
    varchar ma_phong_ban UK
    varchar ten_phong_ban
  }

  lo_tai_san {
    int id PK
    varchar ma_lo UK
    int so_luong
    decimal tong_gia_tri
    timestamp ngay_tao
  }

  tai_khoan_ke_toan {
    int id PK
    varchar ma_tai_khoan UK
    varchar ten_tai_khoan
    varchar loai_tai_khoan
    varchar ma_tai_khoan_cha FK
  }

  chung_tu {
    int id PK
    varchar ma_chung_tu UK
    timestamp ngay_lap
    varchar loai_chung_tu
    decimal tong_tien
    varchar trang_thai
    int nguoi_lap_id
  }

  chi_tiet_chung_tu {
    int id PK
    int chung_tu_id FK
    int tai_san_id FK
    varchar tai_khoan_no FK
    varchar tai_khoan_co FK
    decimal so_tien
    varchar mo_ta
  }

  lich_su_khau_hao {
    int id PK
    int tai_san_id FK
    int chung_tu_id FK
    varchar ky_khau_hao
    decimal so_tien
    decimal luy_ke_sau_kh
    decimal con_lai_sau_kh
    timestamp ngay_tao
  }

  dieu_chuyen_tai_san {
    int id PK
    int tai_san_id FK
    varchar loai_dieu_chuyen
    timestamp ngay_thuc_hien
    int tu_phong_ban_id FK
    int den_phong_ban_id FK
    varchar trang_thai
  }

  bao_tri_tai_san {
    int id PK
    int tai_san_id FK
    varchar loai_bao_tri
    timestamp ngay_thuc_hien
    bool co_chi_phi
    decimal chi_phi
    varchar trang_thai
  }

  thanh_ly_tai_san {
    int id PK
    int tai_san_id FK
    timestamp ngay_thanh_ly
    decimal nguyen_gia
    decimal khau_hao_luy_ke
    decimal gia_tri_con_lai
    decimal gia_tri_thanh_ly
    decimal lai_lo
    varchar ly_do
  }

  tai_san_dinh_kem {
    int id PK
    int tai_san_id FK
    varchar ten_file
    varchar loai_file
    varchar duong_dan
    bigint kich_thuoc
    timestamp ngay_tai
    varchar mo_ta
  }

  tai_san ||--o{ chi_tiet_chung_tu : "1..n"
  tai_san ||--o{ lich_su_khau_hao : "1..n"
  tai_san ||--o{ dieu_chuyen_tai_san : "1..n"
  tai_san ||--o{ bao_tri_tai_san : "1..n"
  tai_san ||--o| thanh_ly_tai_san : "1..1"
  tai_san ||--o{ tai_san_dinh_kem : "1..n"
  tai_san }o--|| danh_muc_tai_san : "n..1"
  tai_san }o--|| phong_ban : "n..1"
  tai_san }o--|| lo_tai_san : "n..1"
  tai_san }o--|| tai_khoan_ke_toan : "n..1"
  chung_tu ||--o{ chi_tiet_chung_tu : "1..n"
  chung_tu ||--o{ lich_su_khau_hao : "1..n"
  danh_muc_tai_san }o--|| tai_khoan_ke_toan : "n..1"
  tai_khoan_ke_toan ||--o{ tai_khoan_ke_toan : "parent-child"
  dieu_chuyen_tai_san }o--|| phong_ban : "tu_phong_ban"
  dieu_chuyen_tai_san }o--|| phong_ban : "den_phong_ban"
`;

// ─── Render mermaid diagram to PNG via puppeteer ─────────────────────────────
async function renderMermaid(diagram, outputFile, width = 2400, height = 1800) {
  const browser = await puppeteer.launch({
    headless: true,
    args: ['--no-sandbox', '--disable-setuid-sandbox', '--disable-web-security'],
  });
  const page = await browser.newPage();
  // deviceScaleFactor: 3 → ảnh nét gấp 3 lần (300 dpi hiệu quả)
  await page.setViewport({ width, height, deviceScaleFactor: 3 });

  const html = `<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<script src="https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js"></script>
<style>
  * { box-sizing: border-box; }
  body {
    margin: 24px;
    background: #ffffff;
    font-family: 'Segoe UI', Arial, sans-serif;
  }
  .mermaid { display: inline-block; }

  /* ── Class diagram: chữ tối, nền rõ ── */
  /* header của class box */
  .classGroup .title { fill: #1e3a5f !important; font-weight: 700 !important; }
  /* title rect nền xanh đậm */
  .classGroup rect.title-state { fill: #d0e4f7 !important; stroke: #2E75B6 !important; }
  /* attribute rows nền trắng/xanh nhạt */
  .classGroup rect.label-box { fill: #f0f6fc !important; stroke: #2E75B6 !important; }
  /* tất cả text trong class diagram → đen đậm */
  .classGroup text,
  .classGroup .label,
  .classGroup tspan { fill: #1a1a1a !important; font-size: 14px !important; }
  /* giữ title màu xanh đậm để phân biệt */
  .classGroup .title tspan { fill: #1e3a5f !important; font-weight: bold !important; font-size: 15px !important; }
  /* mũi tên & đường kẻ */
  .relation { stroke: #2E75B6 !important; stroke-width: 1.5px !important; }
  .relationshipLabel text { fill: #333 !important; font-size: 12px !important; }
  marker path { fill: #2E75B6 !important; }

  /* ── ER diagram: chữ tối, nền rõ ── */
  .er.entityBox { fill: #d0e4f7 !important; stroke: #2E75B6 !important; }
  .er.entityLabel { fill: #1e3a5f !important; font-weight: 700 !important; }
  .er.attributeBoxEven { fill: #f0f6fc !important; stroke: #b0cce8 !important; }
  .er.attributeBoxOdd { fill: #ffffff !important; stroke: #b0cce8 !important; }
  .er text,
  .er tspan { fill: #1a1a1a !important; font-size: 13px !important; }
  .er.entityLabel tspan { fill: #1e3a5f !important; font-size: 14px !important; font-weight: 700 !important; }
  .er.relationshipLabel text { fill: #333 !important; font-size: 12px !important; }
  .er path { stroke: #2E75B6 !important; }
</style>
</head>
<body>
<div class="mermaid">
${diagram}
</div>
<script>
  mermaid.initialize({
    startOnLoad: true,
    theme: 'base',
    themeVariables: {
      /* nền class header: xanh dương nhạt */
      primaryColor: '#d0e4f7',
      /* chữ trên nền primaryColor: xanh đậm */
      primaryTextColor: '#1e3a5f',
      primaryBorderColor: '#2E75B6',
      lineColor: '#2E75B6',
      /* nền attribute rows */
      secondaryColor: '#f0f6fc',
      tertiaryColor: '#ffffff',
      background: '#ffffff',
      mainBkg: '#d0e4f7',
      nodeBorder: '#2E75B6',
      clusterBkg: '#f0f6fc',
      titleColor: '#1e3a5f',
      edgeLabelBackground: '#f5f5f5',
      /* ER diagram */
      attributeBackgroundColorEven: '#f0f6fc',
      attributeBackgroundColorOdd: '#ffffff',
      fontSize: '14px',
      fontFamily: 'Segoe UI, Arial, sans-serif',
    },
    classDiagram: { useMaxWidth: false },
    er: { useMaxWidth: false, diagramPadding: 20 },
  });
</script>
</body>
</html>`;

  await page.setContent(html, { waitUntil: 'domcontentloaded' });
  // Đợi mermaid script load và render xong
  await page.waitForFunction(() => {
    const svg = document.querySelector('.mermaid svg');
    if (!svg) return false;
    // Đảm bảo SVG đã có nội dung thực sự (không chỉ rỗng)
    return svg.querySelectorAll('g').length > 5;
  }, { timeout: 25000, polling: 500 });
  await new Promise(r => setTimeout(r, 2000));

  // Expand SVG to full viewBox size để không bị cắt
  await page.evaluate(() => {
    const svg = document.querySelector('.mermaid svg');
    if (!svg) return;
    const vb = svg.getAttribute('viewBox');
    if (vb) {
      const [, , vw, vh] = vb.split(' ').map(Number);
      if (vw > 0 && vh > 0) {
        svg.setAttribute('width', vw);
        svg.setAttribute('height', vh);
        svg.style.maxWidth = 'none';
        svg.style.width = vw + 'px';
        svg.style.height = vh + 'px';
      }
    }
  });

  // Áp CSS override sau khi render
  await page.addStyleTag({ content: `
    /* Class diagram text colors */
    g.classGroup text { fill: #1a1a1a !important; font-size: 14px !important; }
    g.classGroup .title text,
    g.classGroup .title tspan { fill: #1e3a5f !important; font-weight: 700 !important; font-size: 15px !important; }
    g.classGroup rect { stroke: #2E75B6 !important; }
    /* ER diagram text colors */
    .er text, .er tspan { fill: #1a1a1a !important; }
    .er.entityLabel, .er.entityLabel tspan { fill: #1e3a5f !important; font-weight: 700 !important; }
    .er.entityBox { fill: #d0e4f7 !important; stroke: #2E75B6 !important; }
    .er.attributeBoxEven { fill: #f0f6fc !important; }
    .er.attributeBoxOdd { fill: #ffffff !important; }
    /* Arrows */
    .relation, path.er { stroke: #2E75B6 !important; }
  ` });
  await new Promise(r => setTimeout(r, 800));

  const svgEl = await page.$('.mermaid svg');
  const box = await svgEl.boundingBox();
  console.log(`   SVG box: ${box.width.toFixed(0)}×${box.height.toFixed(0)}px`);

  const pad = 16;
  const png = await page.screenshot({
    clip: {
      x: Math.max(0, box.x - pad),
      y: Math.max(0, box.y - pad),
      width: box.width + pad * 2,
      height: box.height + pad * 2,
    },
    type: 'png',
  });

  writeFileSync(outputFile, png);
  const realW = Math.round((box.width + pad * 2) * 3);
  const realH = Math.round((box.height + pad * 2) * 3);
  console.log(`✅ Rendered: ${outputFile} — PNG ${realW}×${realH}px (3× scale)`);
  await browser.close();
  return { logicalW: box.width, logicalH: box.height };
}

// ─── Word document helpers ────────────────────────────────────────────────────
const shade = (hex) => ({ type: ShadingType.SOLID, color: hex, fill: hex });
const border = () => ({ style: BorderStyle.SINGLE, size: 8, color: '4472C4' });
const allBorders = () => ({ top: border(), bottom: border(), left: border(), right: border(), insideHorizontal: border(), insideVertical: border() });

const H1 = (text) => new Paragraph({ text, heading: HeadingLevel.HEADING_1, spacing: { before: 360, after: 120 } });
const H2 = (text) => new Paragraph({ text, heading: HeadingLevel.HEADING_2, spacing: { before: 240, after: 80 } });
const H3 = (text) => new Paragraph({ text, heading: HeadingLevel.HEADING_3, spacing: { before: 180, after: 60 } });
const P = (text, opts = {}) => new Paragraph({
  children: [new TextRun({ text, size: 24, ...opts })],
  spacing: { after: 120 },
  alignment: AlignmentType.JUSTIFIED,
});
const blank = () => new Paragraph({ text: '' });

const tableHeader = (cols) => new TableRow({
  tableHeader: true,
  children: cols.map(c => new TableCell({
    shading: shade('4472C4'),
    children: [new Paragraph({ children: [new TextRun({ text: c, bold: true, color: 'FFFFFF', size: 22 })], alignment: AlignmentType.CENTER })],
    margins: { top: 60, bottom: 60, left: 80, right: 80 },
  })),
});

function dbTable(caption, tableNum, rows) {
  const widths = [3000, 2500, 2000, 1800];
  return [
    blank(),
    new Table({
      width: { size: 9340, type: WidthType.DXA },
      layout: TableLayoutType.FIXED,
      borders: allBorders(),
      rows: [
        tableHeader(['Tên trường', 'Ký hiệu', 'Kiểu dữ liệu', 'Khóa']),
        ...rows.map((r, i) => new TableRow({
          children: r.map((c, j) => new TableCell({
            shading: i % 2 === 0 ? shade('EBF3FB') : shade('FFFFFF'),
            width: { size: widths[j], type: WidthType.DXA },
            children: [new Paragraph({ children: [new TextRun({ text: String(c ?? ''), size: 22 })] })],
            margins: { top: 40, bottom: 40, left: 80, right: 80 },
          })),
        })),
      ],
    }),
    new Paragraph({
      children: [new TextRun({ text: `Bảng ${tableNum}: ${caption}`, italics: true, size: 22 })],
      alignment: AlignmentType.CENTER,
      spacing: { before: 80, after: 200 },
    }),
  ];
}

function imageSection(imgPath, caption, figNum) {
  const imgBuf = readFileSync(imgPath);
  // Hiển thị ảnh rộng 16cm trong Word (đủ rộng trang A4 trừ lề)
  // docx ImageRun: width/height tính bằng pixel ở 96 dpi → 16cm ≈ 604px, giữ tỉ lệ
  // PNG được render ở 3× scale nên rất nét khi thu nhỏ về 16cm
  return [
    blank(),
    new Paragraph({
      children: [
        new ImageRun({
          data: imgBuf,
          transformation: { width: 620, height: 340 },
          type: 'png',
        }),
      ],
      alignment: AlignmentType.CENTER,
    }),
    new Paragraph({
      children: [new TextRun({ text: `Hình ${figNum}: ${caption}`, italics: true, size: 22, color: '444444' })],
      alignment: AlignmentType.CENTER,
      spacing: { before: 60, after: 240 },
    }),
    blank(),
  ];
}

// ─── Main ─────────────────────────────────────────────────────────────────────
async function main() {
  console.log('🎨 Đang render sơ đồ lớp...');
  await renderMermaid(CLASS_DIAGRAM, 'diagram_class.png', 2000, 2000);

  console.log('🎨 Đang render sơ đồ ERD...');
  await renderMermaid(ER_DIAGRAM, 'diagram_er.png', 2000, 2000);

  console.log('📄 Đang tạo file Word...');

  const doc = new Document({
    creator: 'Hệ thống Quản lý Tài sản Cố định',
    title: 'Tài liệu thiết kế hệ thống - Quản lý TSCĐ',
    styles: {
      default: {
        document: { run: { font: 'Times New Roman', size: 24 } },
      },
    },
    sections: [{
      properties: {},
      children: [

        // ═══════════════ 3.2.1 SƠ ĐỒ LỚP ═══════════════
        H1('3.2. THIẾT KẾ HỆ THỐNG'),
        H2('3.2.1. Sơ đồ lớp (Class Diagram)'),
        P('Dựa trên phân tích nghiệp vụ quản lý tài sản cố định và kiến trúc phần mềm theo mô hình phân lớp (Layered Architecture), hệ thống được thiết kế với 13 lớp thực thể (Entity). Mỗi lớp tương ứng với một bảng trong cơ sở dữ liệu PostgreSQL và được quản lý thông qua Entity Framework Core 8.'),

        ...imageSection('diagram_class.png', 'Sơ đồ lớp (Class Diagram) hệ thống quản lý TSCĐ', '3.29'),

        P('Sơ đồ lớp mô tả các thuộc tính và mối quan hệ giữa các lớp thực thể trong hệ thống. Lớp trung tâm là TaiSan, kết nối với các lớp hỗ trợ nghiệp vụ như ChiTietChungTu, LichSuKhauHao, DieuChuyenTaiSan, BaoTriTaiSan, ThanhLyTaiSan và TaiSanDinhKem. Các lớp danh mục gồm DanhMucTaiSan, PhongBan, TaiKhoanKeToan và LoTaiSan đóng vai trò lookup/reference data.'),

        // ═══════════════ 3.2.2 MÔ HÌNH DỮ LIỆU QUAN HỆ ═══════════════
        H2('3.2.2. Mô hình dữ liệu quan hệ (ER Diagram)'),
        P('Hình 3.30 thể hiện mô hình thực thể - quan hệ (Entity Relationship Diagram - ERD) của hệ thống. Các bảng được liên kết qua khóa ngoại, đảm bảo tính toàn vẹn tham chiếu. Mô hình gồm 13 bảng thuộc schema "asset" và tích hợp với module Auth để quản lý người dùng.'),

        ...imageSection('diagram_er.png', 'Mô hình thực thể - quan hệ (ERD) hệ thống quản lý TSCĐ', '3.30'),

        P('Các quan hệ chính trong mô hình dữ liệu:'),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (n) chi_tiet_chung_tu: Một tài sản có nhiều dòng chi tiết bút toán kế toán.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (n) lich_su_khau_hao: Một tài sản có nhiều kỳ khấu hao được ghi nhận.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (n) dieu_chuyen_tai_san: Một tài sản có thể được điều chuyển nhiều lần.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (n) bao_tri_tai_san: Một tài sản có nhiều lần bảo trì, sửa chữa.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (0..1) thanh_ly_tai_san: Một tài sản tối đa có một biên bản thanh lý.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_san (1) → (n) tai_san_dinh_kem: Một tài sản có nhiều file tài liệu đính kèm.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• chung_tu (1) → (n) chi_tiet_chung_tu: Một chứng từ có nhiều dòng bút toán ghi Nợ/Có.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• tai_khoan_ke_toan (1) → (n) tai_khoan_ke_toan: Tài khoản cấp cha – cấp con (tự tham chiếu).', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• danh_muc_tai_san (1) → (n) tai_san: Một danh mục phân loại nhiều tài sản.', size: 22 })], spacing: { after: 80 } }),
        new Paragraph({ children: [new TextRun({ text: '• phong_ban (1) → (n) tai_san: Một phòng ban quản lý nhiều tài sản.', size: 22 })], spacing: { after: 200 } }),

        // ═══════════════ 3.2.3 THIẾT KẾ VẬT LÝ ═══════════════
        H2('3.2.3. Thiết kế cơ sở dữ liệu vật lý'),
        P('Sử dụng hệ quản trị cơ sở dữ liệu PostgreSQL (Neon Cloud), dựa vào kết quả chuẩn hóa trên, kết hợp tình hình thực tế và yêu cầu người dùng, ta có cơ sở dữ liệu vật lý được thiết kế như sau. Tất cả các bảng thuộc schema "asset".'),

        ...dbTable('Cơ sở vật lý: Tài sản cố định (tai_san)', '3.15', [
          ['Mã tài sản', 'MaTaiSan', 'VARCHAR', 'Khóa chính (UNIQUE)'],
          ['Tên tài sản', 'TenTaiSan', 'VARCHAR(255)', ''],
          ['Mã danh mục', 'DanhMucId', 'INTEGER', 'Khóa ngoại → danh_muc_tai_san'],
          ['Mã phòng ban', 'PhongBanId', 'INTEGER', 'Khóa ngoại → phong_ban'],
          ['Mã lô tài sản', 'LoId', 'INTEGER', 'Khóa ngoại → lo_tai_san'],
          ['Mã tài khoản KT', 'MaTaiKhoan', 'VARCHAR(20)', 'Khóa ngoại → tai_khoan_ke_toan'],
          ['Trạng thái', 'TrangThai', 'VARCHAR(30)', ''],
          ['Số serial', 'SoSeri', 'VARCHAR(100)', ''],
          ['Nhà sản xuất', 'NhaSanXuat', 'VARCHAR(100)', ''],
          ['Ngày mua', 'NgayMua', 'TIMESTAMP', ''],
          ['Nguyên giá', 'NguyenGia', 'DECIMAL(18,2)', ''],
          ['Giá trị còn lại', 'GiaTriConLai', 'DECIMAL(18,2)', ''],
          ['Khấu hao lũy kế', 'KhauHaoLuyKe', 'DECIMAL(18,2)', ''],
          ['KH hàng tháng', 'KhauHaoHangThang', 'DECIMAL(18,2)', ''],
          ['Phương pháp KH', 'PhuongPhapKhauHao', 'VARCHAR(30)', ''],
          ['Thời gian KH (tháng)', 'ThoiGianKhauHao', 'INTEGER', ''],
          ['Người dùng ID', 'NguoiDungId', 'INTEGER', 'Tham chiếu module Auth'],
          ['Ngày cấp phát', 'NgayCapPhat', 'TIMESTAMP', ''],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Danh mục tài sản (danh_muc_tai_san)', '3.16', [
          ['Mã danh mục', 'MaDanhMuc', 'VARCHAR(20)', 'Khóa chính (UNIQUE)'],
          ['Tên danh mục', 'TenDanhMuc', 'VARCHAR(100)', ''],
          ['Tiền tố mã TS', 'TienTo', 'VARCHAR(10)', ''],
          ['Mã tài khoản KT', 'MaTaiKhoan', 'VARCHAR(20)', 'Khóa ngoại → tai_khoan_ke_toan'],
          ['Thời gian KH (tháng)', 'ThoiGianKhauHao', 'INTEGER', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Tài khoản kế toán (tai_khoan_ke_toan)', '3.17', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Mã tài khoản', 'MaTaiKhoan', 'VARCHAR(20)', 'UNIQUE'],
          ['Tên tài khoản', 'TenTaiKhoan', 'VARCHAR(100)', ''],
          ['Loại tài khoản', 'LoaiTaiKhoan', 'VARCHAR(50)', ''],
          ['Mã TK cha', 'MaTaiKhoanCha', 'VARCHAR(20)', 'Khóa ngoại → tai_khoan_ke_toan (self)'],
        ]),

        ...dbTable('Cơ sở vật lý: Phòng ban (phong_ban)', '3.18', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Mã phòng ban', 'MaPhongBan', 'VARCHAR(20)', 'UNIQUE'],
          ['Tên phòng ban', 'TenPhongBan', 'VARCHAR(100)', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Chứng từ kế toán (chung_tu)', '3.19', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Mã chứng từ', 'MaChungTu', 'VARCHAR(50)', 'UNIQUE'],
          ['Ngày lập', 'NgayLap', 'TIMESTAMP', ''],
          ['Loại chứng từ', 'LoaiChungTu', 'VARCHAR(30)', ''],
          ['Mô tả', 'MoTa', 'VARCHAR(500)', ''],
          ['Tổng tiền', 'TongTien', 'DECIMAL(18,2)', ''],
          ['Trạng thái', 'TrangThai', 'VARCHAR(30)', ''],
          ['Người lập ID', 'NguoiLapId', 'INTEGER', 'Tham chiếu module Auth'],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Chi tiết chứng từ (chi_tiet_chung_tu)', '3.20', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Mã chứng từ', 'ChungTuId', 'INTEGER', 'Khóa ngoại → chung_tu'],
          ['Tài khoản Nợ', 'TaiKhoanNo', 'VARCHAR(20)', 'Khóa ngoại → tai_khoan_ke_toan'],
          ['Tài khoản Có', 'TaiKhoanCo', 'VARCHAR(20)', 'Khóa ngoại → tai_khoan_ke_toan'],
          ['Số tiền', 'SoTien', 'DECIMAL(18,2)', ''],
          ['Mô tả', 'MoTa', 'VARCHAR(500)', ''],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san (nullable)'],
        ]),

        ...dbTable('Cơ sở vật lý: Lịch sử khấu hao (lich_su_khau_hao)', '3.21', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san'],
          ['Chứng từ ID', 'ChungTuId', 'INTEGER', 'Khóa ngoại → chung_tu'],
          ['Kỳ khấu hao', 'KyKhauHao', 'VARCHAR(7)', 'Định dạng "YYYY-MM"'],
          ['Số tiền KH', 'SoTien', 'DECIMAL(18,2)', ''],
          ['Lũy kế sau KH', 'LuyKeSauKhauHao', 'DECIMAL(18,2)', ''],
          ['Còn lại sau KH', 'ConLaiSauKhauHao', 'DECIMAL(18,2)', ''],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Điều chuyển tài sản (dieu_chuyen_tai_san)', '3.22', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san'],
          ['Loại điều chuyển', 'LoaiDieuChuyen', 'VARCHAR(30)', ''],
          ['Ngày thực hiện', 'NgayThucHien', 'TIMESTAMP', ''],
          ['Từ phòng ban', 'TuPhongBanId', 'INTEGER', 'Khóa ngoại → phong_ban'],
          ['Đến phòng ban', 'DenPhongBanId', 'INTEGER', 'Khóa ngoại → phong_ban'],
          ['Từ người dùng', 'TuNguoiDungId', 'INTEGER', 'Tham chiếu module Auth'],
          ['Đến người dùng', 'DenNguoiDungId', 'INTEGER', 'Tham chiếu module Auth'],
          ['Trạng thái', 'TrangThai', 'VARCHAR(30)', ''],
          ['Ghi chú', 'GhiChu', 'VARCHAR(500)', ''],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Bảo trì tài sản (bao_tri_tai_san)', '3.23', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san'],
          ['Loại bảo trì', 'LoaiBaoTri', 'VARCHAR(30)', ''],
          ['Ngày thực hiện', 'NgayThucHien', 'TIMESTAMP', ''],
          ['Mô tả', 'MoTa', 'VARCHAR(500)', ''],
          ['Có chi phí?', 'CoChiPhi', 'BOOLEAN', ''],
          ['Chi phí', 'ChiPhi', 'DECIMAL(18,2)', ''],
          ['Loại chi phí', 'LoaiChiPhi', 'VARCHAR(50)', ''],
          ['Nhà cung cấp', 'NhaCungCap', 'VARCHAR(100)', ''],
          ['Trạng thái', 'TrangThai', 'VARCHAR(30)', ''],
          ['Ghi chú', 'GhiChu', 'VARCHAR(500)', ''],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Thanh lý tài sản (thanh_ly_tai_san)', '3.24', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san'],
          ['Ngày thanh lý', 'NgayThanhLy', 'TIMESTAMP', ''],
          ['Nguyên giá', 'NguyenGia', 'DECIMAL(18,2)', ''],
          ['Khấu hao lũy kế', 'KhauHaoLuyKe', 'DECIMAL(18,2)', ''],
          ['Giá trị còn lại', 'GiaTriConLai', 'DECIMAL(18,2)', ''],
          ['Giá trị thanh lý', 'GiaTriThanhLy', 'DECIMAL(18,2)', ''],
          ['Lãi/Lỗ', 'LaiLo', 'DECIMAL(18,2)', ''],
          ['Lý do', 'LyDo', 'VARCHAR(500)', ''],
          ['Ghi chú', 'GhiChu', 'VARCHAR(500)', ''],
          ['Trạng thái', 'TrangThai', 'VARCHAR(30)', ''],
          ['Ngày tạo', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: File đính kèm tài sản (tai_san_dinh_kem)', '3.25', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tài sản ID', 'TaiSanId', 'INTEGER', 'Khóa ngoại → tai_san'],
          ['Tên file', 'TenFile', 'VARCHAR(255)', ''],
          ['Loại file (MIME)', 'LoaiFile', 'VARCHAR(100)', ''],
          ['Đường dẫn lưu trữ', 'DuongDan', 'VARCHAR(500)', ''],
          ['Kích thước (byte)', 'KichThuoc', 'BIGINT', ''],
          ['Ngày tải lên', 'NgayTai', 'TIMESTAMP', ''],
          ['Mô tả', 'MoTa', 'VARCHAR(500)', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Lô tài sản (lo_tai_san)', '3.26', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Mã lô', 'MaLo', 'VARCHAR(30)', 'UNIQUE'],
          ['Số lượng trong lô', 'SoLuong', 'INTEGER', ''],
          ['Tổng giá trị lô', 'TongGiaTri', 'DECIMAL(18,2)', ''],
          ['Ngày tạo lô', 'NgayTao', 'TIMESTAMP', ''],
        ]),

        ...dbTable('Cơ sở vật lý: Cấu hình hệ thống (cau_hinh_he_thong)', '3.27', [
          ['ID', 'Id', 'INTEGER', 'Khóa chính (tự tăng)'],
          ['Tên công ty', 'TenCongTy', 'VARCHAR(200)', ''],
          ['Mã số thuế', 'MaSoThue', 'VARCHAR(20)', ''],
          ['Địa chỉ', 'DiaChi', 'VARCHAR(500)', ''],
          ['Tiền tố chứng từ', 'TienToChungTu', 'VARCHAR(10)', ''],
          ['Số bắt đầu CT', 'SoBatDauChungTu', 'INTEGER', ''],
          ['PP KH mặc định', 'PhuongPhapKhauHaoMacDinh', 'VARCHAR(30)', ''],
          ['Tự động KH', 'TuDongKhauHao', 'BOOLEAN', ''],
          ['Định dạng mã TS', 'DinhDangMaTaiSan', 'VARCHAR(50)', ''],
          ['Độ dài mã TS', 'DoDaiMaTaiSan', 'INTEGER', ''],
        ]),

        // ═══════════════ 3.3 KIẾN TRÚC ═══════════════
        H1('3.3. KIẾN TRÚC VÀ XÂY DỰNG PHẦN MỀM QUẢN LÝ TÀI SẢN CỐ ĐỊNH'),
        H2('3.3.1. Kiến trúc hệ thống, công nghệ phát triển phần mềm'),
        P('Phần mềm quản lý tài sản cố định được xây dựng trên nền tảng công nghệ hiện đại, đảm bảo tính ổn định, dễ bảo trì và mở rộng trong tương lai. Hệ thống được thiết kế theo mô hình kiến trúc N-lớp (N-tier architecture) kết hợp Clean Architecture, bao gồm:'),
        new Paragraph({ children: [new TextRun({ text: '• Lớp giao diện (Presentation Layer):', bold: true, size: 22 }), new TextRun({ text: ' Ứng dụng web SPA (Single Page Application) xây dựng bằng React 18 + TypeScript + Vite. Giao diện được thiết kế với Shadcn/Radix UI và Tailwind CSS, cho phép người dùng tương tác trực tiếp với hệ thống qua trình duyệt web.', size: 22 })], spacing: { after: 120 } }),
        new Paragraph({ children: [new TextRun({ text: '• Lớp API (API Gateway Layer):', bold: true, size: 22 }), new TextRun({ text: ' ASP.NET Core 8 Web API theo kiến trúc RESTful. Bao gồm các Controller xử lý yêu cầu HTTP, xác thực token JWT, phân quyền RBAC (Role-Based Access Control). Chia thành 2 module độc lập: module Auth và module Asset.', size: 22 })], spacing: { after: 120 } }),
        new Paragraph({ children: [new TextRun({ text: '• Lớp xử lý nghiệp vụ (Application Service Layer):', bold: true, size: 22 }), new TextRun({ text: ' Chứa các Service class thực thi logic nghiệp vụ. Tuân theo nguyên tắc Dependency Injection (DI), các Service được đăng ký và inject qua interface.', size: 22 })], spacing: { after: 120 } }),
        new Paragraph({ children: [new TextRun({ text: '• Lớp hạ tầng (Infrastructure Layer):', bold: true, size: 22 }), new TextRun({ text: ' Entity Framework Core 8 với provider Npgsql (PostgreSQL). AssetDbContext quản lý toàn bộ mapping, migration, và seed data.', size: 22 })], spacing: { after: 120 } }),
        new Paragraph({ children: [new TextRun({ text: '• Lớp domain (Domain Layer):', bold: true, size: 22 }), new TextRun({ text: ' Định nghĩa các Entity class với Data Annotation và Fluent API. Bao gồm các Enum: TrangThaiTaiSan, PhuongPhapKhauHao, LoaiChungTu, LoaiDieuChuyen, TrangThaiBaoTri, TrangThaiThanhLy.', size: 22 })], spacing: { after: 200 } }),

        P('Toàn bộ phần mềm được phát triển trong môi trường Visual Studio 2022 (backend) và VS Code (frontend). Hệ thống được triển khai trên nền tảng đám mây: backend deploy trên Fly.io (Docker container), database PostgreSQL sử dụng dịch vụ Neon Cloud, frontend deploy trên Vercel.'),

        blank(),
        H2('3.3.2. Các công nghệ và thư viện chính'),
        ...dbTable('Công nghệ và thư viện sử dụng trong hệ thống', '3.28', [
          ['ASP.NET Core 8', '.NET 8 Web API', 'Backend framework', 'Microsoft'],
          ['Entity Framework Core 8', 'ORM + Migrations', 'Data access', 'Microsoft'],
          ['PostgreSQL (Neon)', 'Serverless PostgreSQL', 'Database', 'Neon.tech'],
          ['React 18 + TypeScript', 'SPA Framework', 'Frontend UI', 'Meta / Microsoft'],
          ['Vite 6', 'Build tool', 'Frontend bundler', 'Evan You'],
          ['Tailwind CSS', 'Utility-first CSS', 'Styling', 'Tailwind Labs'],
          ['Shadcn/Radix UI', 'Component library', 'UI Components', 'shadcn'],
          ['JWT (JSON Web Token)', 'Authentication', 'Auth & Security', 'RFC 7519'],
          ['SheetJS (xlsx)', 'Excel export', 'Reporting', 'SheetJS'],
          ['docx', 'Word document gen', 'Reporting', 'Open Source'],
          ['Fly.io', 'Container hosting', 'Deployment', 'Fly.io'],
          ['Docker', 'Containerization', 'DevOps', 'Docker Inc.'],
          ['Vercel', 'Static hosting', 'Frontend deploy', 'Vercel Inc.'],
        ]),
      ],
    }],
  });

  const buf = await Packer.toBuffer(doc);
  writeFileSync('TaiLieu_ThietKe_HeThong_TSCD_v3.docx', buf);
  console.log('✅  Đã tạo: TaiLieu_ThietKe_HeThong_TSCD_v3.docx');
}

main().catch(err => { console.error('❌ Lỗi:', err); process.exit(1); });
