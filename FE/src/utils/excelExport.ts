import * as XLSX from 'xlsx';

// ── Format helpers ────────────────────────────────────────────────────────────
const fmtDate = (d?: string | null) =>
  d ? new Date(d).toLocaleDateString('vi-VN') : '';

// Set a cell value (helper to reduce repetition)
function sc(
  ws: XLSX.WorkSheet,
  r: number,
  c: number,
  v: string | number | null | undefined,
) {
  if (v === null || v === undefined || v === '') return;
  const addr = XLSX.utils.encode_cell({ r, c });
  const t: XLSX.ExcelDataType = typeof v === 'number' ? 'n' : 's';
  ws[addr] = { v, t };
}

// Add a merge range (0-indexed)
function mg(
  list: XLSX.Range[],
  r1: number, c1: number,
  r2: number, c2: number,
) {
  list.push({ s: { r: r1, c: c1 }, e: { r: r2, c: c2 } });
}

// Finalise the worksheet ref and merges
function finalise(
  ws: XLSX.WorkSheet,
  merges: XLSX.Range[],
  maxRow: number,
  maxCol: number,
) {
  ws['!ref'] = XLSX.utils.encode_range({ s: { r: 0, c: 0 }, e: { r: maxRow, c: maxCol } });
  ws['!merges'] = merges;
}


// ═══════════════════════════════════════════════════════════════════════════════
// Mẫu S03b-DN: Sổ Cái
// Cols: A(0) B(1) C(2) D(3) E(4) F(5) G(6) H(7) I(8)
// ═══════════════════════════════════════════════════════════════════════════════
export function exportSoCai(
  data: {
    maTaiKhoan: string;
    tenTaiKhoan?: string;
    soDuDauKy: number;
    phatSinhNo: number;
    phatSinhCo: number;
    soDuCuoiKy: number;
    butToans: {
      ngayHachToan?: string;
      maChungTu?: string;
      dienGiai?: string;
      phatSinhNo: number;
      phatSinhCo: number;
      soDuLuyKe?: number;
    }[];
  },
  fromDate: string,
  toDate: string,
) {
  const ws: XLSX.WorkSheet = {};
  const M: XLSX.Range[] = [];
  const year = fromDate ? new Date(fromDate).getFullYear() : new Date().getFullYear();

  // ── Row 0 (Excel 1) ──
  sc(ws, 0, 0, 'Đơn vị:.....................................');
  sc(ws, 0, 5, 'Mẫu số S03b-DN');
  mg(M, 0, 5, 0, 8);                       // F1:I1

  // ── Row 1 (Excel 2) ──
  sc(ws, 1, 0, 'Địa chỉ:....................................');
  sc(ws, 1, 5, '(Kèm theo Thông tư số 99/2025/TT-BTC ngày 27\r\ntháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)');
  mg(M, 1, 5, 2, 8);                       // F2:I3

  // ── Row 4 (Excel 5) ──
  sc(ws, 4, 0, 'SỔ CÁI');
  mg(M, 4, 0, 4, 8);

  // ── Row 5 (Excel 6) ──
  sc(ws, 5, 0, '(Dùng cho hình thức kế toán Nhật ký chung)');
  mg(M, 5, 0, 5, 8);

  // ── Row 6 (Excel 7) ──
  sc(ws, 6, 0, `Năm ${year}`);
  mg(M, 6, 0, 6, 8);

  // ── Row 7 (Excel 8) ──
  sc(ws, 7, 0, `Tên tài khoản: ${data.tenTaiKhoan ?? '...........'}`);
  mg(M, 7, 0, 7, 8);

  // ── Row 8 (Excel 9) ──
  sc(ws, 8, 0, `Số hiệu: ${data.maTaiKhoan}`);
  mg(M, 8, 0, 8, 8);

  // ── Row 10 (Excel 11) — header row 1 ──
  sc(ws, 10, 0, 'Ngày,\r\ntháng\r\nghi sổ');  mg(M, 10, 0, 11, 0);   // A11:A12
  sc(ws, 10, 1, 'Chứng từ');                   mg(M, 10, 1, 10, 2);   // B11:C11
  sc(ws, 10, 3, 'Diễn giải');                  mg(M, 10, 3, 11, 3);   // D11:D12
  sc(ws, 10, 4, 'Nhật ký chung');              mg(M, 10, 4, 10, 5);   // E11:F11
  sc(ws, 10, 6, 'Số hiệu\r\nTK đối\r\nứng');  mg(M, 10, 6, 11, 6);   // G11:G12
  sc(ws, 10, 7, 'Số tiền');                    mg(M, 10, 7, 10, 8);   // H11:I11

  // ── Row 11 (Excel 12) — header row 2 ──
  sc(ws, 11, 1, 'Số\r\nhiệu');
  sc(ws, 11, 2, 'Ngày\r\ntháng');
  sc(ws, 11, 4, 'Trang\r\nsổ');
  sc(ws, 11, 5, 'STT\r\ndòng');
  sc(ws, 11, 7, 'Nợ');
  sc(ws, 11, 8, 'Có');

  // ── Row 12 (Excel 13) — column index ──
  ['A','B','C','D','E','G','H','1','2'].forEach((v, c) => sc(ws, 12, c, v));

  // ── Row 13 (Excel 14) — Số dư đầu kỳ ──
  sc(ws, 13, 3, '- Số dư đầu năm');
  if (data.soDuDauKy >= 0) sc(ws, 13, 7, data.soDuDauKy || 0);
  else                      sc(ws, 13, 8, Math.abs(data.soDuDauKy));

  // ── Row 14 (Excel 15) — section label ──
  sc(ws, 14, 3, '- Số phát sinh trong tháng');

  // ── Data rows (start at index 15) ──
  const DATA_START = 15;
  data.butToans.forEach((b, i) => {
    const r = DATA_START + i;
    sc(ws, r, 0, fmtDate(b.ngayHachToan));
    sc(ws, r, 1, b.maChungTu ?? '');
    sc(ws, r, 2, fmtDate(b.ngayHachToan));
    sc(ws, r, 3, b.dienGiai ?? '');
    if (b.phatSinhNo > 0) sc(ws, r, 7, b.phatSinhNo);
    if (b.phatSinhCo > 0) sc(ws, r, 8, b.phatSinhCo);
  });

  const AF = DATA_START + data.butToans.length; // After data row

  // ── Cộng phát sinh ──
  sc(ws, AF, 3, '- Cộng số phát sinh tháng');
  sc(ws, AF, 7, data.phatSinhNo);
  sc(ws, AF, 8, data.phatSinhCo);

  // ── Số dư cuối kỳ ──
  sc(ws, AF + 1, 3, '- Số dư cuối tháng');
  if (data.soDuCuoiKy >= 0) sc(ws, AF + 1, 7, data.soDuCuoiKy || 0);
  else                       sc(ws, AF + 1, 8, Math.abs(data.soDuCuoiKy));

  // ── Cộng lũy kế ──
  sc(ws, AF + 2, 3, '- Cộng lũy kế từ đầu quý');

  // ── Footer ──
  const F0 = AF + 4;
  sc(ws, F0, 0, '- Sổ này có .... trang, đánh số từ trang số 01 đến trang ....');
  mg(M, F0, 0, F0, 8);
  sc(ws, F0 + 1, 0, '- Ngày mở sổ:....');
  mg(M, F0 + 1, 0, F0 + 1, 8);

  sc(ws, F0 + 3, 6, 'Ngày..... tháng.... năm.....');
  mg(M, F0 + 3, 6, F0 + 3, 8);

  sc(ws, F0 + 4, 0, '');   mg(M, F0 + 4, 0, F0 + 4, 2);   // A:C
  sc(ws, F0 + 4, 3, 'Kế toán trưởng');
  mg(M, F0 + 4, 3, F0 + 4, 5);
  sc(ws, F0 + 4, 6, 'Người đại diện theo pháp luật');
  mg(M, F0 + 4, 6, F0 + 4, 8);

  sc(ws, F0 + 5, 3, '(Ký, họ tên)');
  mg(M, F0 + 5, 3, F0 + 5, 5);
  sc(ws, F0 + 5, 6, '(Ký, họ tên, đóng dấu)');
  mg(M, F0 + 5, 6, F0 + 5, 8);

  finalise(ws, M, F0 + 5, 8);
  ws['!cols'] = [
    { wch: 14 }, { wch: 14 }, { wch: 12 }, { wch: 38 },
    { wch: 10 }, { wch: 10 }, { wch: 12 }, { wch: 18 }, { wch: 18 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Sổ Cái');
  XLSX.writeFile(wb, `So_Cai_${data.maTaiKhoan}_${fromDate ? fromDate.substring(0,7) : ''}.xlsx`);
}


// ═══════════════════════════════════════════════════════════════════════════════
// Mẫu S03a-DN: Sổ Nhật Ký Chung
// Cols: A(0) B(1) C(2) D(3) E(4) F(5) G(6) H(7) I(8)
// ═══════════════════════════════════════════════════════════════════════════════
export function exportNhatKyChung(
  entries: {
    ngayGhiSo?: string;
    ngayLap?: string;
    maChungTu?: string;
    dienGiai?: string;
    taiKhoanNo?: string;
    taiKhoanCo?: string;
    soTien?: number;
  }[],
  fromDate: string,
  toDate: string,
) {
  const ws: XLSX.WorkSheet = {};
  const M: XLSX.Range[] = [];
  const year = fromDate ? new Date(fromDate).getFullYear() : new Date().getFullYear();

  // ── Row 0 (Excel 1) ──
  sc(ws, 0, 0, 'Đơn vị:.....................................');
  sc(ws, 0, 4, 'Mẫu số S03a-DN');
  mg(M, 0, 4, 0, 8);                       // E1:I1

  // ── Row 1 (Excel 2) ──
  sc(ws, 1, 0, 'Địa chỉ:....................................');
  sc(ws, 1, 4, '(Kèm theo Thông tư số 99/2025/TT-BTC ngày 27 tháng 10\r\nnăm 2025 của Bộ trưởng Bộ Tài chính)');
  mg(M, 1, 4, 2, 8);                       // E2:I3

  // ── Row 4 (Excel 5) ──
  sc(ws, 4, 0, 'SỔ NHẬT KÝ CHUNG');
  mg(M, 4, 0, 4, 8);

  // ── Row 5 (Excel 6) ──
  sc(ws, 5, 0, `Năm ${year}`);
  mg(M, 5, 0, 5, 8);

  // ── Row 6 (Excel 7) ──
  sc(ws, 6, 7, 'Đơn vị tính:...........');
  mg(M, 6, 7, 6, 8);                       // H7:I7

  // ── Row 8 (Excel 9) — header row 1 ──
  sc(ws, 8, 0, 'Ngày,\r\ntháng\r\nghi sổ');   mg(M, 8, 0, 9, 0);   // A9:A10
  sc(ws, 8, 1, 'Chứng từ');                    mg(M, 8, 1, 8, 2);   // B9:C9
  sc(ws, 8, 3, 'Diễn giải');                   mg(M, 8, 3, 9, 3);   // D9:D10
  sc(ws, 8, 4, 'Đã ghi\r\nSổ Cái');            mg(M, 8, 4, 9, 4);   // E9:E10
  sc(ws, 8, 5, 'STT\r\ndòng');                 mg(M, 8, 5, 9, 5);   // F9:F10
  sc(ws, 8, 6, 'Số hiệu\r\nTK đối\r\nứng');   mg(M, 8, 6, 9, 6);   // G9:G10
  sc(ws, 8, 7, 'Số phát sinh');                mg(M, 8, 7, 8, 8);   // H9:I9

  // ── Row 9 (Excel 10) — header row 2 ──
  sc(ws, 9, 1, 'Số\r\nhiệu');
  sc(ws, 9, 2, 'Ngày,\r\ntháng');
  sc(ws, 9, 7, 'Nợ');
  sc(ws, 9, 8, 'Có');

  // ── Row 10 (Excel 11) — column index ──
  ['A','B','C','D','E','G','H','1','2'].forEach((v, c) => sc(ws, 10, c, v));

  // ── Row 11 (Excel 12) — "Số trang trước chuyển sang" ──
  sc(ws, 11, 3, 'Số trang trước chuyển sang');

  // ── Data rows (start at index 12) ──
  const DATA_START = 12;
  entries.forEach((e, i) => {
    const r = DATA_START + i;
    sc(ws, r, 0, fmtDate(e.ngayGhiSo ?? e.ngayLap));
    sc(ws, r, 1, e.maChungTu ?? '');
    sc(ws, r, 2, fmtDate(e.ngayLap));
    sc(ws, r, 3, e.dienGiai ?? '');
    sc(ws, r, 6, e.taiKhoanNo ? `${e.taiKhoanNo}/${e.taiKhoanCo ?? ''}` : '');
    if (e.soTien && e.soTien > 0) {
      sc(ws, r, 7, e.soTien);   // Nợ
      sc(ws, r, 8, e.soTien);   // Có (double-entry: same amount)
    }
  });

  const AF = DATA_START + entries.length;
  const total = entries.reduce((s, e) => s + (e.soTien ?? 0), 0);

  // ── "Cộng chuyển sang trang sau" ──
  sc(ws, AF, 3, 'Cộng chuyển sang trang sau');
  sc(ws, AF, 4, 'x');
  sc(ws, AF, 5, 'x');
  sc(ws, AF, 6, 'x');
  sc(ws, AF, 7, total);
  sc(ws, AF, 8, total);

  // ── Footer ──
  const F0 = AF + 2;
  sc(ws, F0, 0, '- Sổ này có .... trang, đánh số từ trang số 01 đến trang ...');
  mg(M, F0, 0, F0, 8);
  sc(ws, F0 + 1, 0, '- Ngày mở sổ:....');
  mg(M, F0 + 1, 0, F0 + 1, 8);

  sc(ws, F0 + 3, 6, 'Ngày..... tháng.... năm.....');
  mg(M, F0 + 3, 6, F0 + 3, 8);

  sc(ws, F0 + 4, 0, '');  mg(M, F0 + 4, 0, F0 + 4, 2);
  sc(ws, F0 + 4, 3, 'Kế toán trưởng');
  mg(M, F0 + 4, 3, F0 + 4, 5);
  sc(ws, F0 + 4, 6, 'Người đại diện theo pháp luật');
  mg(M, F0 + 4, 6, F0 + 4, 8);

  sc(ws, F0 + 5, 3, '(Ký, họ tên)');
  mg(M, F0 + 5, 3, F0 + 5, 5);
  sc(ws, F0 + 5, 6, '(Ký, họ tên, đóng dấu)');
  mg(M, F0 + 5, 6, F0 + 5, 8);

  finalise(ws, M, F0 + 5, 8);
  ws['!cols'] = [
    { wch: 14 }, { wch: 14 }, { wch: 12 }, { wch: 38 },
    { wch: 10 }, { wch: 10 }, { wch: 16 }, { wch: 18 }, { wch: 18 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Sổ Nhật Ký Chung');
  const d = fromDate ? fromDate.substring(0, 7) : '';
  XLSX.writeFile(wb, `So_Nhat_Ky_Chung_${d}.xlsx`);
}


// ═══════════════════════════════════════════════════════════════════════════════
// Mẫu S23-DN: Thẻ Tài Sản Cố Định
// Cols: A(0) B(1) C(2) D(3) E(4) F(5) G(6)
// ═══════════════════════════════════════════════════════════════════════════════
export function exportTheeTSCD(asset: any, categories: any[], departments: any[]) {
  const getCat = (id?: number) => categories.find(c => c.id === id)?.tenDanhMuc ?? '';
  const getDept = (id?: number) => departments.find(d => d.id === id)?.tenPhongBan ?? '';
  const ppkh = asset.phuongPhapKhauHao === 0 ? 'Đường thẳng'
    : asset.phuongPhapKhauHao === 1 ? 'Số dư giảm dần'
    : 'Theo số lượng sản phẩm';

  const ws: XLSX.WorkSheet = {};
  const M: XLSX.Range[] = [];

  const today = new Date();
  const dd = today.getDate(), mm = today.getMonth() + 1, yyyy = today.getFullYear();
  const ngayMuaYear = asset.ngayMua ? new Date(asset.ngayMua).getFullYear() : '';

  // ── Row 0 (Excel 1) ──
  sc(ws, 0, 0, 'Đơn vị: ................................');
  sc(ws, 0, 3, 'Mẫu số S23-DN');
  mg(M, 0, 3, 0, 6);                       // D1:G1

  // ── Row 1 (Excel 2) ──
  sc(ws, 1, 0, 'Địa chỉ: ...............................');
  sc(ws, 1, 3, '(Kèm theo Thông tư số 99/2025/TT-BTC \r\nngày 27 tháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)');
  mg(M, 1, 3, 2, 6);                       // D2:G3

  // ── Row 4 (Excel 5) ──
  sc(ws, 4, 0, 'THẺ TÀI SẢN CỐ ĐỊNH');
  mg(M, 4, 0, 4, 6);

  // ── Row 5 (Excel 6) ──
  sc(ws, 5, 0, `Số: ${asset.maTaiSan ?? '.....................'}`);
  mg(M, 5, 0, 5, 6);

  // ── Row 6 (Excel 7) ──
  sc(ws, 6, 0, `Ngày ${dd} tháng ${mm} năm ${yyyy} lập thẻ`);
  mg(M, 6, 0, 6, 6);

  // ── Row 8 (Excel 9) ──
  sc(ws, 8, 0, 'Căn cứ vào Biên bản giao nhận TSCĐ số. .................... ngày.... tháng.... năm...');
  mg(M, 8, 0, 8, 6);

  // ── Row 9 (Excel 10) ──
  sc(ws, 9, 0, `Tên, ký mã hiệu, quy cách (cấp hạng) TSCĐ: ${asset.tenTaiSan ?? ''} - Số hiệu TSCĐ: ${asset.maTaiSan ?? ''}`);
  mg(M, 9, 0, 9, 6);

  // ── Row 10 (Excel 11) ──
  sc(ws, 10, 0, `Nước sản xuất (xây dựng): ${asset.nhaSanXuat ?? '.............'} - Năm sản xuất: ${ngayMuaYear}`);
  mg(M, 10, 0, 10, 6);

  // ── Row 11 (Excel 12) ──
  sc(ws, 11, 0, `Bộ phận quản lý, sử dụng: ${getDept(asset.phongBanId)} - Năm đưa vào sử dụng: ${asset.ngayCapPhat ? new Date(asset.ngayCapPhat).getFullYear() : '...'}`);
  mg(M, 11, 0, 11, 6);

  // ── Row 12 (Excel 13) ──
  sc(ws, 12, 0, `Công suất (diện tích thiết kế): ........................................`);
  mg(M, 12, 0, 12, 6);

  // ── Row 13 (Excel 14) ──
  sc(ws, 13, 0, 'Đình chỉ sử dụng TSCĐ ngày ............. tháng ................. năm ...................');
  mg(M, 13, 0, 13, 6);

  // ── Row 14 (Excel 15) ──
  sc(ws, 14, 0, `Lý do đình chỉ: .....................................................`);
  mg(M, 14, 0, 14, 6);

  // ── Row 16 (Excel 17) — depreciation table header row 1 ──
  sc(ws, 16, 0, 'Số hiệu\r\nchứng từ');  mg(M, 16, 0, 17, 0);   // A17:A18
  sc(ws, 16, 1, 'Nguyên giá tài sản cố định');  mg(M, 16, 1, 16, 3);  // B17:D17
  sc(ws, 16, 4, 'Giá trị hao mòn tài sản cố định');  mg(M, 16, 4, 16, 6); // E17:G17

  // ── Row 17 (Excel 18) — depreciation table header row 2 ──
  sc(ws, 17, 1, 'Ngày, tháng,\r\nnăm');
  sc(ws, 17, 2, 'Diễn\r\ngiải');
  sc(ws, 17, 3, 'Nguyên\r\ngiá');
  sc(ws, 17, 4, 'Năm');
  sc(ws, 17, 5, 'Giá trị hao\r\nmòn');
  sc(ws, 17, 6, 'Cộng dồn');

  // ── Row 18 (Excel 19) — column index ──
  sc(ws, 18, 0, 'A');
  sc(ws, 18, 1, 'B');
  sc(ws, 18, 2, 'C');
  sc(ws, 18, 3, '1');
  sc(ws, 18, 4, '2');
  sc(ws, 18, 5, '3');
  sc(ws, 18, 6, '4');

  // ── Row 19 (Excel 20) — first data row (opening / current value) ──
  sc(ws, 19, 1, fmtDate(asset.ngayMua));
  sc(ws, 19, 2, `Ghi tăng: ${getCat(asset.danhMucId)}`);
  sc(ws, 19, 3, asset.nguyenGia ?? 0);
  sc(ws, 19, 4, ppkh);
  sc(ws, 19, 5, asset.khauHaoHangThang ?? 0);
  sc(ws, 19, 6, asset.khauHaoLuyKe ?? 0);

  // ── Row 22 (Excel 23) — accessories section ──
  sc(ws, 22, 0, 'Dụng cụ phụ tùng kèm theo');
  mg(M, 22, 0, 22, 6);

  // ── Row 23 (Excel 24) — accessories header ──
  sc(ws, 23, 0, 'Số\r\nTT');
  sc(ws, 23, 1, 'Tên, quy cách dụng\r\ncụ, phụ tùng');  mg(M, 23, 1, 23, 2);  // B24:C24
  sc(ws, 23, 3, 'Đơn vị\r\ntính');
  sc(ws, 23, 4, 'Số lượng');
  sc(ws, 23, 5, 'Giá trị');  mg(M, 23, 5, 23, 6);  // F24:G24

  // ── Row 24 (Excel 25) — accessories index ──
  sc(ws, 24, 0, 'A');
  sc(ws, 24, 1, 'B');  mg(M, 24, 1, 24, 2);
  sc(ws, 24, 3, 'C');
  sc(ws, 24, 4, '1');
  sc(ws, 24, 5, '2');  mg(M, 24, 5, 24, 6);

  // ── Rows 25–28: empty accessory data rows ──

  // ── Row 29 (Excel 30) ──
  sc(ws, 29, 0, 'Ghi giảm TSCĐ chứng từ số: ....... ngày .... tháng .... năm ............................');
  mg(M, 29, 0, 29, 6);

  // ── Row 31 (Excel 32) ──
  sc(ws, 31, 0, 'Lý do giảm: ....................................................');
  mg(M, 31, 0, 31, 6);

  // ── Row 33 (Excel 34) ──
  sc(ws, 33, 4, 'Ngày..... tháng.... năm ....');
  mg(M, 33, 4, 33, 6);                     // E34:G34

  // ── Row 34 (Excel 35) — signature labels ──
  sc(ws, 34, 0, 'Người ghi sổ');     mg(M, 34, 0, 34, 1);  // A35:B35
  sc(ws, 34, 2, 'Kế toán trưởng');   mg(M, 34, 2, 34, 3);  // C35:D35
  sc(ws, 34, 4, 'Người đại diện theo pháp luật');  mg(M, 34, 4, 34, 6); // E35:G35

  // ── Row 35 (Excel 36) — signature lines ──
  sc(ws, 35, 0, '(Ký, họ tên)');    mg(M, 35, 0, 35, 1);  // A36:B36
  sc(ws, 35, 2, '(Ký, họ tên)');    mg(M, 35, 2, 35, 3);  // C36:D36
  sc(ws, 35, 4, '(Ký, họ tên, đóng dấu)');  mg(M, 35, 4, 35, 6); // E36:G36

  finalise(ws, M, 35, 6);
  ws['!cols'] = [
    { wch: 14 }, { wch: 18 }, { wch: 28 }, { wch: 18 },
    { wch: 22 }, { wch: 18 }, { wch: 18 },
  ];
  ws['!rows'] = Array.from({ length: 36 }, (_, i) =>
    [4, 5, 6, 22].includes(i) ? { hpt: 24 } : { hpt: 18 }
  );

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Thẻ TSCĐ');
  XLSX.writeFile(wb, `The_TSCD_${asset.maTaiSan ?? 'TS'}.xlsx`);
}


// ═══════════════════════════════════════════════════════════════════════════════
// Bảng Trích Khấu Hao TSCĐ
// Cols: A(0) B(1) C(2) D(3) E(4) F(5) G(6)
// ═══════════════════════════════════════════════════════════════════════════════
export function exportBangTinhKhauHao(assets: any[], period?: string) {
  const ws: XLSX.WorkSheet = {};
  const M: XLSX.Range[] = [];

  // ── Row 0 (Excel 1) ──
  sc(ws, 0, 0, 'Đơn vị: ….............');

  // ── Row 1 (Excel 2) ──
  sc(ws, 1, 0, 'Địa chỉ: …............');

  // ── Row 3 (Excel 4) ──
  sc(ws, 3, 0, 'BẢNG TRÍCH KHẤU HAO TÀI SẢN CỐ ĐỊNH');
  mg(M, 3, 0, 3, 6);

  // ── Row 4 (Excel 5) — kỳ ──
  if (period) {
    sc(ws, 4, 0, `Kỳ: ${period}`);
    mg(M, 4, 0, 4, 6);
  }

  // ── Row 5 (Excel 6) — column headers ──
  sc(ws, 5, 0, 'Mã TSCĐ');
  sc(ws, 5, 1, 'Tên TSCĐ');
  sc(ws, 5, 2, 'Ngày sử dụng');
  sc(ws, 5, 3, 'Nguyên giá');
  sc(ws, 5, 4, 'Hao mòn trong kỳ');
  sc(ws, 5, 5, 'Hao mòn lũy kế');
  sc(ws, 5, 6, 'Giá trị còn lại');

  // ── Row 6 (Excel 7) — column index ──
  sc(ws, 6, 0, 'A');
  sc(ws, 6, 1, 'B');
  sc(ws, 6, 2, 'C');
  sc(ws, 6, 3, '1');
  sc(ws, 6, 4, '2');
  sc(ws, 6, 5, '3');
  sc(ws, 6, 6, '4');

  // ── Data rows (start at index 7) ──
  const DATA_START = 7;
  assets.forEach((a, i) => {
    const r = DATA_START + i;
    sc(ws, r, 0, a.maTaiSan ?? '');
    sc(ws, r, 1, a.tenTaiSan ?? '');
    sc(ws, r, 2, fmtDate(a.ngayCapPhat ?? a.ngayMua));
    sc(ws, r, 3, a.nguyenGia ?? 0);
    sc(ws, r, 4, a.khauHaoHangThang ?? 0);
    sc(ws, r, 5, a.khauHaoLuyKe ?? 0);
    sc(ws, r, 6, (a.nguyenGia ?? 0) - (a.khauHaoLuyKe ?? 0));
  });

  // ── Total row ──
  const TR = DATA_START + assets.length;
  sc(ws, TR, 1, 'Cộng');
  sc(ws, TR, 3, assets.reduce((s, a) => s + (a.nguyenGia ?? 0), 0));
  sc(ws, TR, 4, assets.reduce((s, a) => s + (a.khauHaoHangThang ?? 0), 0));
  sc(ws, TR, 5, assets.reduce((s, a) => s + (a.khauHaoLuyKe ?? 0), 0));
  sc(ws, TR, 6, assets.reduce((s, a) => s + ((a.nguyenGia ?? 0) - (a.khauHaoLuyKe ?? 0)), 0));

  // ── Footer (signature rows) ──
  const F0 = TR + 2;
  sc(ws, F0, 1, 'Giám đốc');
  sc(ws, F0, 3, 'Kế toán trưởng');
  sc(ws, F0, 6, 'Người lập');

  sc(ws, F0 + 1, 1, '(Ký, họ tên, đóng dấu)');
  sc(ws, F0 + 1, 3, '(Ký, họ tên)');
  sc(ws, F0 + 1, 6, '(Ký, họ tên)');

  finalise(ws, M, F0 + 1, 6);
  ws['!cols'] = [
    { wch: 12 }, { wch: 32 }, { wch: 14 }, { wch: 18 },
    { wch: 18 }, { wch: 18 }, { wch: 18 },
  ];

  const wb = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Bảng KH TSCĐ');
  XLSX.writeFile(wb, `Bang_Trich_KH_TSCD_${period ?? new Date().toISOString().substring(0,7)}.xlsx`);
}
