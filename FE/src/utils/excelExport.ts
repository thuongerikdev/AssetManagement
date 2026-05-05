import * as XLSX from 'xlsx';

const fmt = (n?: number | null) =>
  n == null ? '' : n.toLocaleString('vi-VN', { minimumFractionDigits: 0, maximumFractionDigits: 0 });

const fmtDate = (d?: string | null) =>
  d ? new Date(d).toLocaleDateString('vi-VN') : '';

// ─── Mẫu S23-DN: Thẻ Tài Sản Cố Định ───────────────────────────────────────
export function exportTheeTSCD(asset: any, categories: any[], departments: any[]) {
  const getCat = (id?: number) => categories.find(c => c.id === id)?.tenDanhMuc ?? '';
  const getDept = (id?: number) => departments.find(d => d.id === id)?.tenPhongBan ?? '';

  const rows: (string | number | null)[][] = [
    ['THẺ TÀI SẢN CỐ ĐỊNH', null, null, null, null, null],
    ['Mẫu số S23-DN'],
    [],
    ['Ngày ... tháng ... năm ...'],
    [],
    ['Tên tài sản cố định:', asset.tenTaiSan],
    ['Mã tài sản:', asset.maTaiSan],
    ['Đơn vị sử dụng:', getDept(asset.phongBanId)],
    ['Số hiệu tài sản:', asset.soSeri ?? ''],
    ['Danh mục:', getCat(asset.danhMucId)],
    ['Nhà sản xuất:', asset.nhaSanXuat ?? ''],
    ['Ngày mua:', fmtDate(asset.ngayMua)],
    ['Ngày đưa vào sử dụng:', fmtDate(asset.ngayCapPhat)],
    [],
    ['THÔNG SỐ KỸ THUẬT'],
    [asset.thongSoKyThuat ?? ''],
    [],
    ['GIÁ TRỊ TÀI SẢN'],
    ['', 'Nguyên giá', 'Hao mòn lũy kế', 'Giá trị còn lại'],
    [
      'Theo sổ kế toán',
      asset.nguyenGia ?? 0,
      asset.khauHaoLuyKe ?? 0,
      (asset.nguyenGia ?? 0) - (asset.khauHaoLuyKe ?? 0),
    ],
    [],
    ['KHẤU HAO TÀI SẢN'],
    ['Phương pháp KH:', asset.phuongPhapKhauHao == 0 ? 'Đường thẳng' : asset.phuongPhapKhauHao == 1 ? 'Số dư giảm dần' : 'Theo số lượng'],
    ['Thời gian KH (tháng):', asset.thoiGianKhauHao ?? ''],
    ['Khấu hao hàng tháng:', asset.khauHaoHangThang ?? 0],
    [],
    ['Ghi chú:', asset.moTa ?? ''],
  ];

  const ws = XLSX.utils.aoa_to_sheet(rows);
  ws['!cols'] = [{ wch: 30 }, { wch: 20 }, { wch: 20 }, { wch: 20 }];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Thẻ TSCĐ');
  XLSX.writeFile(wb, `The_TSCD_${asset.maTaiSan}_${asset.tenTaiSan}.xlsx`);
}

// ─── Bảng Tính & Phân Bổ Khấu Hao TSCĐ ─────────────────────────────────────
export function exportBangTinhKhauHao(assets: any[], period?: string) {
  const header = [
    'Mã TSCĐ',
    'Tên TSCĐ',
    'Ngày đưa vào SD',
    'Nguyên giá',
    'Thời gian KH (tháng)',
    'KH hàng tháng',
    'Hao mòn trong kỳ',
    'Hao mòn lũy kế',
    'Giá trị còn lại',
  ];

  const rows = assets.map(a => [
    a.maTaiSan,
    a.tenTaiSan ?? '',
    fmtDate(a.ngayCapPhat ?? a.ngayMua),
    a.nguyenGia ?? 0,
    a.thoiGianKhauHao ?? '',
    a.khauHaoHangThang ?? 0,
    a.khauHaoHangThang ?? 0,
    a.khauHaoLuyKe ?? 0,
    (a.nguyenGia ?? 0) - (a.khauHaoLuyKe ?? 0),
  ]);

  const totalRow = [
    'TỔNG CỘNG', '', '',
    assets.reduce((s, a) => s + (a.nguyenGia ?? 0), 0),
    '', '',
    assets.reduce((s, a) => s + (a.khauHaoHangThang ?? 0), 0),
    assets.reduce((s, a) => s + (a.khauHaoLuyKe ?? 0), 0),
    assets.reduce((s, a) => s + ((a.nguyenGia ?? 0) - (a.khauHaoLuyKe ?? 0)), 0),
  ];

  const ws = XLSX.utils.aoa_to_sheet([
    ['BẢNG TÍNH VÀ PHÂN BỔ KHẤU HAO TÀI SẢN CỐ ĐỊNH'],
    [`Kỳ: ${period ?? new Date().toLocaleDateString('vi-VN')}`],
    [],
    header,
    ...rows,
    totalRow,
  ]);

  ws['!cols'] = [
    { wch: 12 }, { wch: 30 }, { wch: 16 }, { wch: 16 },
    { wch: 14 }, { wch: 16 }, { wch: 16 }, { wch: 16 }, { wch: 16 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Bảng KH TSCĐ');
  XLSX.writeFile(wb, `Bang_Tinh_KH_TSCD_${period ?? ''}.xlsx`);
}

// ─── Mẫu S03b-DN: Sổ Cái ────────────────────────────────────────────────────
export function exportSoCai(data: {
  maTaiKhoan: string;
  tenTaiKhoan?: string;
  soDuDauKy: number;
  phatSinhNo: number;
  phatSinhCo: number;
  soDuCuoiKy: number;
  butToans: any[];
}, fromDate: string, toDate: string) {
  const rows: (string | number | null)[][] = [
    ['SỔ CÁI', null, null, null, null, null, null, null],
    ['Mẫu số S03b-DN — Theo Thông tư 99/2025/TT-BTC'],
    [`Tài khoản ${data.maTaiKhoan} — ${data.tenTaiKhoan ?? ''}`],
    [`Từ ${fmtDate(fromDate)} đến ${fmtDate(toDate)}`],
    [],
    [
      'Ngày ghi sổ', 'Số chứng từ', 'Ngày chứng từ',
      'Diễn giải', 'Trang sổ NKC', 'STT dòng NKC',
      'TK đối ứng', 'Phát sinh Nợ', 'Phát sinh Có',
    ],
    [
      '', '', '',
      'Số dư đầu kỳ', '', '',
      '', '', fmt(data.soDuDauKy),
    ],
    ...data.butToans.map(b => [
      fmtDate(b.ngayHachToan),
      b.maChungTu ?? '',
      fmtDate(b.ngayHachToan),
      b.dienGiai ?? '',
      '',
      '',
      '',
      b.phatSinhNo > 0 ? b.phatSinhNo : '',
      b.phatSinhCo > 0 ? b.phatSinhCo : '',
    ]),
    [
      '', '', '',
      'Cộng phát sinh', '', '',
      '', data.phatSinhNo, data.phatSinhCo,
    ],
    [
      '', '', '',
      'Số dư cuối kỳ', '', '',
      '', '', data.soDuCuoiKy,
    ],
  ];

  const ws = XLSX.utils.aoa_to_sheet(rows);
  ws['!cols'] = [
    { wch: 14 }, { wch: 16 }, { wch: 14 }, { wch: 36 },
    { wch: 12 }, { wch: 12 }, { wch: 12 }, { wch: 16 }, { wch: 16 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, `Sổ Cái ${data.maTaiKhoan}`);
  XLSX.writeFile(wb, `So_Cai_${data.maTaiKhoan}_${fromDate}_${toDate}.xlsx`);
}

// ─── Mẫu S03a-DN: Sổ Nhật Ký Chung ─────────────────────────────────────────
export function exportNhatKyChung(entries: any[], fromDate: string, toDate: string) {
  const header = [
    'Ngày ghi sổ', 'Số hiệu CT', 'Ngày tháng CT',
    'Diễn giải', 'Trang sổ trước', 'STT dòng',
    'TK Nợ', 'TK Có', 'Số tiền',
  ];

  const rows = entries.map(e => [
    fmtDate(e.ngayGhiSo ?? e.ngayLap),
    e.soHieuCT ?? e.maChungTu ?? '',
    fmtDate(e.ngayLap),
    e.dienGiai ?? e.moTa ?? '',
    '',
    '',
    e.taiKhoanNo ?? '',
    e.taiKhoanCo ?? '',
    e.soTien ?? 0,
  ]);

  const ws = XLSX.utils.aoa_to_sheet([
    ['SỔ NHẬT KÝ CHUNG', null, null, null, null, null, null, null, null],
    ['Mẫu số S03a-DN — Theo Thông tư 99/2025/TT-BTC'],
    [`Từ ${fmtDate(fromDate)} đến ${fmtDate(toDate)}`],
    [],
    header,
    ...rows,
    ['TỔNG CỘNG', '', '', '', '', '', '', '', entries.reduce((s, e) => s + (e.soTien ?? 0), 0)],
  ]);

  ws['!cols'] = [
    { wch: 14 }, { wch: 16 }, { wch: 14 }, { wch: 36 },
    { wch: 12 }, { wch: 12 }, { wch: 12 }, { wch: 12 }, { wch: 16 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Nhật Ký Chung');
  XLSX.writeFile(wb, `So_Nhat_Ky_Chung_${fromDate}_${toDate}.xlsx`);
}
