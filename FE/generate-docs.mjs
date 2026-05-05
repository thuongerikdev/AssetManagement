import {
  Document, Packer, Paragraph, Table, TableRow, TableCell,
  TextRun, HeadingLevel, AlignmentType, WidthType,
  BorderStyle, ShadingType, convertInchesToTwip, PageOrientation,
  TableLayoutType
} from 'docx';
import { writeFileSync } from 'fs';

// ── helpers ────────────────────────────────────────────────────────────────
const shade = (hex) => ({ type: ShadingType.SOLID, color: hex, fill: hex });
const border = () => ({ style: BorderStyle.SINGLE, size: 8, color: '4472C4' });
const noBorder = () => ({ style: BorderStyle.NONE, size: 0, color: 'FFFFFF' });
const allBorders = () => ({ top: border(), bottom: border(), left: border(), right: border(), insideHorizontal: border(), insideVertical: border() });

const H1 = (text) => new Paragraph({
  text, heading: HeadingLevel.HEADING_1,
  spacing: { before: 360, after: 120 },
  run: { color: '1F3864', bold: true },
});
const H2 = (text) => new Paragraph({
  text, heading: HeadingLevel.HEADING_2,
  spacing: { before: 240, after: 80 },
});
const H3 = (text) => new Paragraph({
  text, heading: HeadingLevel.HEADING_3,
  spacing: { before: 180, after: 60 },
});
const P = (text, opts = {}) => new Paragraph({
  children: [new TextRun({ text, size: 24, ...opts })],
  spacing: { after: 120 },
  alignment: AlignmentType.JUSTIFIED,
});
const blank = () => new Paragraph({ text: '' });

// Header row for DB table
const tableHeader = (cols) => new TableRow({
  tableHeader: true,
  children: cols.map(c => new TableCell({
    shading: shade('4472C4'),
    children: [new Paragraph({
      children: [new TextRun({ text: c, bold: true, color: 'FFFFFF', size: 22 })],
      alignment: AlignmentType.CENTER,
    })],
    margins: { top: 60, bottom: 60, left: 80, right: 80 },
  })),
});

// Data row
const tableRow = (cells, isEven = false) => new TableRow({
  children: cells.map(c => new TableCell({
    shading: isEven ? shade('DCE6F1') : shade('FFFFFF'),
    children: [new Paragraph({
      children: [new TextRun({ text: String(c ?? ''), size: 22 })],
    })],
    margins: { top: 40, bottom: 40, left: 80, right: 80 },
  })),
});

// Full DB table with caption
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
            children: [new Paragraph({
              children: [new TextRun({
                text: String(c ?? ''),
                size: 22,
                bold: j === 1 && c && (c.includes('PK') || c.includes('FK')),
              })],
            })],
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

// ── CLASS DIAGRAM (text-based UML box) ─────────────────────────────────────
function classBox(name, pkFields, fkFields, otherFields) {
  const mkRow = (label, fields) => fields.length === 0 ? [] : [
    new TableRow({
      children: [new TableCell({
        columnSpan: 2,
        shading: shade('D6E4F0'),
        children: [new Paragraph({ children: [new TextRun({ text: label, bold: true, size: 20, color: '1F3864' })] })],
        margins: { top: 30, bottom: 30, left: 80, right: 80 },
      })],
    }),
    ...fields.map(f => new TableRow({
      children: [
        new TableCell({
          width: { size: 600, type: WidthType.DXA },
          shading: shade('FFFFFF'),
          children: [new Paragraph({ children: [new TextRun({ text: f.tag || '', size: 18, color: '7030A0', bold: true })] })],
          margins: { top: 20, bottom: 20, left: 80, right: 40 },
        }),
        new TableCell({
          shading: shade('FFFFFF'),
          children: [new Paragraph({ children: [new TextRun({ text: f.name, size: 20 })] })],
          margins: { top: 20, bottom: 20, left: 40, right: 80 },
        }),
      ],
    })),
  ];

  return new Table({
    width: { size: 4200, type: WidthType.DXA },
    borders: allBorders(),
    rows: [
      new TableRow({
        children: [new TableCell({
          columnSpan: 2,
          shading: shade('2E75B6'),
          children: [new Paragraph({
            children: [new TextRun({ text: name, bold: true, color: 'FFFFFF', size: 24 })],
            alignment: AlignmentType.CENTER,
          })],
          margins: { top: 60, bottom: 60, left: 80, right: 80 },
        })],
      }),
      ...mkRow('Khóa chính (PK)', pkFields),
      ...mkRow('Khóa ngoại (FK)', fkFields),
      ...mkRow('Thuộc tính', otherFields),
    ],
  });
}

// ── CONTENT ────────────────────────────────────────────────────────────────
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

      // ══════════════════════════════════════════════════════════════════════
      // CHƯƠNG 3 HEADER
      // ══════════════════════════════════════════════════════════════════════
      H1('3.2. THIẾT KẾ HỆ THỐNG'),
      H2('3.2.1. Sơ đồ lớp (Class Diagram)'),
      P('Dựa trên phân tích nghiệp vụ quản lý tài sản cố định và kiến trúc phần mềm theo mô hình phân lớp (Layered Architecture), hệ thống được thiết kế với các lớp thực thể (Entity) như sau. Mỗi lớp tương ứng với một bảng trong cơ sở dữ liệu PostgreSQL và được quản lý thông qua Entity Framework Core.'),

      blank(),

      // Row 1 – TaiSan + ChungTu
      new Table({
        width: { size: 9340, type: WidthType.DXA },
        borders: { top: noBorder(), bottom: noBorder(), left: noBorder(), right: noBorder(), insideHorizontal: noBorder(), insideVertical: noBorder() },
        rows: [new TableRow({ children: [
          new TableCell({ children: [classBox('TaiSan (tai_san)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'DanhMucId → DanhMucTaiSan' }, { tag: 'FK2', name: 'PhongBanId → PhongBan' }, { tag: 'FK3', name: 'LoId → LoTaiSan' }, { tag: 'FK4', name: 'MaTaiKhoan → TaiKhoanKeToan' }],
            [{ name: 'MaTaiSan : string' }, { name: 'TenTaiSan : string?' }, { name: 'TrangThai : enum' }, { name: 'SoSeri : string?' }, { name: 'NhaSanXuat : string?' }, { name: 'NgayMua : DateTime?' }, { name: 'NguyenGia : decimal?' }, { name: 'GiaTriConLai : decimal?' }, { name: 'KhauHaoLuyKe : decimal?' }, { name: 'KhauHaoHangThang : decimal?' }, { name: 'PhuongPhapKhauHao : enum?' }, { name: 'ThoiGianKhauHao : int?' }, { name: 'NguoiDungId : int?' }, { name: 'NgayCapPhat : DateTime?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('ChungTu (chung_tu)',
            [{ tag: 'PK', name: 'Id : int' }],
            [],
            [{ name: 'MaChungTu : string' }, { name: 'NgayLap : DateTime?' }, { name: 'LoaiChungTu : enum?' }, { name: 'MoTa : string?' }, { name: 'TongTien : decimal?' }, { name: 'TrangThai : string?' }, { name: 'NguoiLapId : int?' }, { name: 'NgayTao : DateTime?' }],
          )] }),
        ]})],
      }),

      blank(),

      // Row 2 – ChiTietChungTu + LichSuKhauHao
      new Table({
        width: { size: 9340, type: WidthType.DXA },
        borders: { top: noBorder(), bottom: noBorder(), left: noBorder(), right: noBorder(), insideHorizontal: noBorder(), insideVertical: noBorder() },
        rows: [new TableRow({ children: [
          new TableCell({ children: [classBox('ChiTietChungTu (chi_tiet_chung_tu)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'ChungTuId → ChungTu' }, { tag: 'FK2', name: 'TaiSanId → TaiSan' }, { tag: 'FK3', name: 'TaiKhoanNo → TaiKhoanKeToan' }, { tag: 'FK4', name: 'TaiKhoanCo → TaiKhoanKeToan' }],
            [{ name: 'SoTien : decimal?' }, { name: 'MoTa : string?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('LichSuKhauHao (lich_su_khau_hao)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'TaiSanId → TaiSan' }, { tag: 'FK2', name: 'ChungTuId → ChungTu' }],
            [{ name: 'KyKhauHao : string?' }, { name: 'SoTien : decimal?' }, { name: 'LuyKeSauKhauHao : decimal?' }, { name: 'ConLaiSauKhauHao : decimal?' }, { name: 'NgayTao : DateTime?' }],
          )] }),
        ]})],
      }),

      blank(),

      // Row 3 – DieuChuyenTaiSan + BaoTriTaiSan
      new Table({
        width: { size: 9340, type: WidthType.DXA },
        borders: { top: noBorder(), bottom: noBorder(), left: noBorder(), right: noBorder(), insideHorizontal: noBorder(), insideVertical: noBorder() },
        rows: [new TableRow({ children: [
          new TableCell({ children: [classBox('DieuChuyenTaiSan (dieu_chuyen_tai_san)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'TaiSanId → TaiSan' }, { tag: 'FK2', name: 'TuPhongBanId → PhongBan' }, { tag: 'FK3', name: 'DenPhongBanId → PhongBan' }],
            [{ name: 'LoaiDieuChuyen : enum?' }, { name: 'NgayThucHien : DateTime?' }, { name: 'TuNguoiDungId : int?' }, { name: 'DenNguoiDungId : int?' }, { name: 'TrangThai : string?' }, { name: 'GhiChu : string?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('BaoTriTaiSan (bao_tri_tai_san)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'TaiSanId → TaiSan' }],
            [{ name: 'LoaiBaoTri : string?' }, { name: 'NgayThucHien : DateTime?' }, { name: 'MoTa : string?' }, { name: 'CoChiPhi : bool?' }, { name: 'ChiPhi : decimal?' }, { name: 'NhaCungCap : string?' }, { name: 'TrangThai : enum?' }],
          )] }),
        ]})],
      }),

      blank(),

      // Row 4 – ThanhLyTaiSan + TaiSanDinhKem
      new Table({
        width: { size: 9340, type: WidthType.DXA },
        borders: { top: noBorder(), bottom: noBorder(), left: noBorder(), right: noBorder(), insideHorizontal: noBorder(), insideVertical: noBorder() },
        rows: [new TableRow({ children: [
          new TableCell({ children: [classBox('ThanhLyTaiSan (thanh_ly_tai_san)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'TaiSanId → TaiSan' }],
            [{ name: 'NgayThanhLy : DateTime?' }, { name: 'NguyenGia : decimal?' }, { name: 'KhauHaoLuyKe : decimal?' }, { name: 'GiaTriConLai : decimal?' }, { name: 'GiaTriThanhLy : decimal?' }, { name: 'LaiLo : decimal?' }, { name: 'LyDo : string?' }, { name: 'TrangThai : enum?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('TaiSanDinhKem (tai_san_dinh_kem)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'TaiSanId → TaiSan' }],
            [{ name: 'TenFile : string' }, { name: 'LoaiFile : string?' }, { name: 'DuongDan : string?' }, { name: 'KichThuoc : long?' }, { name: 'NgayTai : DateTime' }, { name: 'MoTa : string?' }],
          )] }),
        ]})],
      }),

      blank(),

      // Row 5 – DanhMucTaiSan + PhongBan + TaiKhoanKeToan
      new Table({
        width: { size: 9340, type: WidthType.DXA },
        borders: { top: noBorder(), bottom: noBorder(), left: noBorder(), right: noBorder(), insideHorizontal: noBorder(), insideVertical: noBorder() },
        rows: [new TableRow({ children: [
          new TableCell({ children: [classBox('DanhMucTaiSan (danh_muc_tai_san)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'MaTaiKhoan → TaiKhoanKeToan' }],
            [{ name: 'MaDanhMuc : string' }, { name: 'TenDanhMuc : string?' }, { name: 'TienTo : string?' }, { name: 'ThoiGianKhauHao : int?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('PhongBan (phong_ban)',
            [{ tag: 'PK', name: 'Id : int' }],
            [],
            [{ name: 'MaPhongBan : string' }, { name: 'TenPhongBan : string?' }],
          )], margins: { right: convertInchesToTwip(0.2) } }),
          new TableCell({ children: [classBox('TaiKhoanKeToan (tai_khoan_ke_toan)',
            [{ tag: 'PK', name: 'Id : int' }],
            [{ tag: 'FK1', name: 'MaTaiKhoanCha → self' }],
            [{ name: 'MaTaiKhoan : string' }, { name: 'TenTaiKhoan : string?' }, { name: 'LoaiTaiKhoan : string?' }],
          )] }),
        ]})],
      }),

      blank(),

      // ── Mô tả quan hệ ──────────────────────────────────────────────────
      H2('3.2.2. Thiết kế mô hình dữ liệu quan hệ'),
      P('Hình 3.30 thể hiện mô hình dữ liệu quan hệ (ERD) của hệ thống quản lý tài sản cố định. Các bảng được liên kết thông qua khóa ngoại, đảm bảo tính toàn vẹn dữ liệu. Mô hình gồm 13 bảng chính thuộc module Asset và tích hợp với module Auth để quản lý người dùng.'),
      blank(),
      P('Các quan hệ chính trong mô hình:'),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (n) ChiTietChungTu: Một tài sản có nhiều dòng chi tiết chứng từ kế toán.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (n) LichSuKhauHao: Một tài sản có nhiều kỳ khấu hao được ghi nhận.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (n) DieuChuyenTaiSan: Một tài sản có thể được điều chuyển nhiều lần.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (n) BaoTriTaiSan: Một tài sản có nhiều lần bảo trì, sửa chữa.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (1) ThanhLyTaiSan: Một tài sản tối đa có một biên bản thanh lý.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiSan (1) → (n) TaiSanDinhKem: Một tài sản có nhiều file tài liệu đính kèm.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• ChungTu (1) → (n) ChiTietChungTu: Một chứng từ có nhiều dòng bút toán ghi Nợ/Có.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• TaiKhoanKeToan (1) → (n) TaiKhoanKeToan: Tài khoản cấp cha – cấp con (tự tham chiếu).', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• DanhMucTaiSan (1) → (n) TaiSan: Một danh mục phân loại nhiều tài sản.', size: 22 })], spacing: { after: 80 } }),
      new Paragraph({ children: [new TextRun({ text: '• PhongBan (1) → (n) TaiSan: Một phòng ban quản lý nhiều tài sản.', size: 22 })], spacing: { after: 200 } }),

      // ══════════════════════════════════════════════════════════════════════
      // 3.2.2.2 VẬT LÝ
      // ══════════════════════════════════════════════════════════════════════
      H2('3.2.2.1. Thiết kế cơ sở dữ liệu vật lý'),
      P('Sử dụng hệ quản trị cơ sở dữ liệu PostgreSQL (Neon Cloud), dựa vào kết quả chuẩn hóa trên, kết hợp tình hình thực tế và yêu cầu người dùng, ta có cơ sở dữ liệu vật lý được thiết kế như sau. Tất cả các bảng thuộc schema "asset", trừ bảng người dùng thuộc schema "auth".'),

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

      // ══════════════════════════════════════════════════════════════════════
      // 3.3 KIẾN TRÚC HỆ THỐNG
      // ══════════════════════════════════════════════════════════════════════
      H1('3.3. KIẾN TRÚC VÀ XÂY DỰNG PHẦN MỀM QUẢN LÝ TÀI SẢN CỐ ĐỊNH'),
      H2('3.3.1. Kiến trúc hệ thống, công nghệ phát triển phần mềm'),
      P('Phần mềm quản lý tài sản cố định được xây dựng trên nền tảng công nghệ hiện đại, đảm bảo tính ổn định, dễ bảo trì và mở rộng trong tương lai. Hệ thống được thiết kế theo mô hình kiến trúc N-lớp (N-tier architecture) kết hợp Clean Architecture, bao gồm:'),
      new Paragraph({ children: [new TextRun({ text: '• Lớp giao diện (Presentation Layer):', bold: true, size: 22 }), new TextRun({ text: ' Ứng dụng web SPA (Single Page Application) xây dựng bằng React 18 + TypeScript + Vite. Giao diện được thiết kế với Shadcn/Radix UI và Tailwind CSS, cho phép người dùng tương tác trực tiếp với hệ thống qua trình duyệt web. Bao gồm các chức năng: ghi tăng, trích khấu hao, điều chuyển, bảo trì, thanh lý tài sản, sổ cái kế toán, báo cáo theo dõi TSCĐ.', size: 22 })], spacing: { after: 120 } }),
      new Paragraph({ children: [new TextRun({ text: '• Lớp API (API Gateway Layer):', bold: true, size: 22 }), new TextRun({ text: ' ASP.NET Core 8 Web API theo kiến trúc RESTful. Bao gồm các Controller xử lý yêu cầu HTTP từ FE, xác thực token JWT, phân quyền RBAC (Role-Based Access Control). Chia thành 2 module độc lập: module Auth (quản lý người dùng, phân quyền) và module Asset (nghiệp vụ tài sản).', size: 22 })], spacing: { after: 120 } }),
      new Paragraph({ children: [new TextRun({ text: '• Lớp xử lý nghiệp vụ (Application Service Layer):', bold: true, size: 22 }), new TextRun({ text: ' Chứa các Service class thực thi logic nghiệp vụ: TaiSanService, ChungTuService, SoCaiService, DepreciationService... Tuân theo nguyên tắc Dependency Injection (DI), các Service được đăng ký và inject qua interface, dễ dàng kiểm thử và thay thế.', size: 22 })], spacing: { after: 120 } }),
      new Paragraph({ children: [new TextRun({ text: '• Lớp hạ tầng (Infrastructure Layer):', bold: true, size: 22 }), new TextRun({ text: ' Entity Framework Core 8 với provider Npgsql (PostgreSQL). AssetDbContext quản lý toàn bộ mapping, migration, và seed data. DbContext được cấu hình với Value Converter cho các kiểu DateTime (UTC), Enum → string, và các ràng buộc toàn vẹn dữ liệu (cascade delete, restrict).', size: 22 })], spacing: { after: 120 } }),
      new Paragraph({ children: [new TextRun({ text: '• Lớp domain (Domain Layer):', bold: true, size: 22 }), new TextRun({ text: ' Định nghĩa các Entity class với Data Annotation và Fluent API. Bao gồm các Enum: TrangThaiTaiSan, PhuongPhapKhauHao, LoaiChungTu, LoaiDieuChuyen, TrangThaiBaoTri, TrangThaiThanhLy.', size: 22 })], spacing: { after: 200 } }),

      P('Toàn bộ phần mềm được phát triển trong môi trường Visual Studio 2022 (backend) và VS Code (frontend). Hệ thống được triển khai trên nền tảng đám mây: backend deploy trên Fly.io (Docker container), database PostgreSQL sử dụng dịch vụ Neon Cloud (serverless PostgreSQL), frontend deploy trên Vercel. Việc containerization bằng Docker đảm bảo tính nhất quán môi trường và khả năng scale theo nhu cầu.'),

      blank(),
      H2('3.3.2. Các công nghệ và thư viện chính'),
      ...dbTable('Công nghệ và thư viện sử dụng', '3.28', [
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

Packer.toBuffer(doc).then(buf => {
  writeFileSync('TaiLieu_ThietKe_HeThong_TSCD.docx', buf);
  console.log('✅  Đã tạo: TaiLieu_ThietKe_HeThong_TSCD.docx');
});
