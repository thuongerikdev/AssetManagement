import {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  WidthType, AlignmentType, BorderStyle, VerticalAlign,
  PageOrientation
} from "docx";
import { saveAs } from "file-saver";
import { TaiSan } from "../api/assetApi";
import { Department } from "../api/departmentApi";
import { AssetCategory } from "../api/assetCategoryApi";
import { CauHinhHeThong } from "../api/systemConfigApi";

// Hàm hỗ trợ format tiền
const formatVND = (value?: number) => {
  if (value === undefined || value === null) return "";
  return new Intl.NumberFormat('vi-VN').format(value);
};

export const exportTheTSCDWord = async (
  asset: TaiSan,
  categories: AssetCategory[],
  departments: Department[],
  config?: CauHinhHeThong
) => {
  const deptName = departments.find(d => d.id === asset.phongBanId)?.tenPhongBan || "........................................";
  
  const purchaseYear = asset.ngayMua ? new Date(asset.ngayMua).getFullYear().toString() : "........";
  const allocationYear = asset.ngayCapPhat ? new Date(asset.ngayCapPhat).getFullYear().toString() : "........";
  
  const today = new Date();
  const day = today.getDate().toString().padStart(2, '0');
  const month = (today.getMonth() + 1).toString().padStart(2, '0');
  const year = today.getFullYear();

  // Định dạng viền ẩn cho các bảng dùng để căn lề
  const noBorders = {
    top: { style: BorderStyle.NONE, size: 0, color: "auto" },
    bottom: { style: BorderStyle.NONE, size: 0, color: "auto" },
    left: { style: BorderStyle.NONE, size: 0, color: "auto" },
    right: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideHorizontal: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideVertical: { style: BorderStyle.NONE, size: 0, color: "auto" },
  };

  const doc = new Document({
    sections: [{
      properties: {
        page: {
          margin: { top: 1000, right: 1000, bottom: 1000, left: 1500 },
        },
      },
      children: [
        // HEADER: Đơn vị / Mẫu số
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: 55, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Đơn vị: ", bold: true }),
                        new TextRun({ text: config?.tenCongTy || "..........................................." })
                      ]
                    }),
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Mã số thuế: ", bold: true }),
                        new TextRun({ text: config?.maSoThue || "..................." })
                      ]
                    }),
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Địa chỉ: ", bold: true }),
                        new TextRun({ text: config?.diaChi || ".........................................." })
                      ]
                    }),
                  ],
                }),
                new TableCell({
                  width: { size: 45, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Mẫu số S23-DN", bold: true })] }),
                    new Paragraph({
                      alignment: AlignmentType.CENTER,
                      children: [
                        new TextRun({ text: "(Kèm theo Thông tư số 99/2025/TT-BTC", italics: true, size: 20 }),
                        new TextRun({ text: "ngày 27 tháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)", italics: true, size: 20, break: 1 })
                      ]
                    }),
                  ],
                }),
              ],
            }),
          ],
        }),

        new Paragraph({ text: "", spacing: { after: 200 } }),

        // TÊN TIÊU ĐỀ
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: "THẺ TÀI SẢN CỐ ĐỊNH", bold: true, size: 32 })],
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: `Số: ${asset.maTaiSan}`, italics: true })],
          spacing: { after: 100 }
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: `Ngày ${day} tháng ${month} năm ${year} lập thẻ`, italics: true })],
          spacing: { after: 200 }
        }),

        // THÔNG TIN CHUNG TÀI SẢN
        new Paragraph({ children: [new TextRun("Căn cứ vào Biên bản giao nhận TSCĐ số ......................... ngày ........ tháng ........ năm ........")] }),
        new Paragraph({ spacing: { before: 100 }, children: [
          new TextRun("Tên, ký mã hiệu, quy cách (cấp hạng) TSCĐ: "),
          new TextRun({ text: asset.tenTaiSan || "........................................", bold: true }),
          new TextRun("   Số hiệu TSCĐ: "),
          new TextRun({ text: asset.maTaiSan, bold: true }),
        ]}),
        new Paragraph({ spacing: { before: 100 }, children: [
          new TextRun("Nước sản xuất (xây dựng): "),
          new TextRun({ text: asset.nhaSanXuat || "..........................." }),
          new TextRun("        Năm sản xuất: "),
          new TextRun({ text: purchaseYear }),
        ]}),
        new Paragraph({ spacing: { before: 100 }, children: [
          new TextRun("Bộ phận quản lý, sử dụng: "),
          new TextRun({ text: deptName }),
          new TextRun("        Năm đưa vào sử dụng: "),
          new TextRun({ text: allocationYear }),
        ]}),
        new Paragraph({ spacing: { before: 100 }, children: [new TextRun("Công suất (diện tích thiết kế): ................................................................................................")] }),
        new Paragraph({ spacing: { before: 100 }, children: [new TextRun("Đình chỉ sử dụng TSCĐ ngày ........ tháng ........ năm ........")] }),
        new Paragraph({ spacing: { before: 100, after: 200 }, children: [new TextRun("Lý do đình chỉ: ............................................................................................................................")] }),

        // BẢNG 1: NGUYÊN GIÁ VÀ HAO MÒN
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          rows: [
            new TableRow({
              children: [
                new TableCell({ columnSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số hiệu chứng từ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Diễn giải", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Nguyên giá tài sản cố định", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ columnSpan: 3, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Giá trị hao mòn tài sản cố định", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
              ],
            }),
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số, hiệu", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Ngày, tháng, năm", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Năm", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Giá trị hao mòn", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Cộng dồn", bold: true })] })] }),
              ],
            }),
            // Dòng dữ liệu mẫu 1
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "Ghi tăng" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: asset.ngayMua ? new Date(asset.ngayMua).toLocaleDateString('vi-VN') : "" })] }),
                new TableCell({ children: [new Paragraph({ text: " Mua mới/Đưa vào SD" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.nguyenGia) })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: year.toString() })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.khauHaoHangThang) })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.khauHaoLuyKe) })] }),
              ],
            }),
            // Dòng trống chừa chỗ ghi chép
            ...Array.from({ length: 3 }).map(() => new TableRow({
              children: Array.from({ length: 7 }).map(() => new TableCell({ children: [new Paragraph({ text: "" })] }))
            }))
          ],
        }),

        new Paragraph({
          children: [new TextRun({ text: "Dụng cụ phụ tùng kèm theo", bold: true })],
          spacing: { before: 200, after: 100 }
        }),

        // BẢNG 2: PHỤ TÙNG KÈM THEO
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          rows: [
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số TT", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Tên, quy cách dụng cụ, phụ tùng", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Đơn vị tính", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số lượng", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Giá trị", bold: true })] })] }),
              ],
            }),
            // Dòng trống
            ...Array.from({ length: 3 }).map(() => new TableRow({
              children: Array.from({ length: 5 }).map(() => new TableCell({ children: [new Paragraph({ text: "" })] }))
            }))
          ],
        }),

        // FOOTER & CHỮ KÝ
        new Paragraph({ spacing: { before: 200 }, children: [new TextRun("Ghi giảm TSCĐ chứng từ số: .................... ngày ........ tháng ........ năm ............................")] }),
        new Paragraph({ spacing: { before: 100 }, children: [new TextRun("Lý do giảm: ................................................................................................................................")] }),
        
        new Paragraph({
          alignment: AlignmentType.RIGHT,
          spacing: { before: 200, after: 100 },
          children: [new TextRun({ text: `Ngày ${day} tháng ${month} năm ${year}`, italics: true })]
        }),

        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người ghi sổ", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Kế toán trưởng", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 34, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người đại diện theo pháp luật", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên, đóng dấu)", italics: true })] }),
                  ],
                }),
              ],
            }),
          ],
        }),
      ],
    }],
  });

  const buffer = await Packer.toBlob(doc);
  saveAs(buffer, `The_TSCD_${asset.maTaiSan}.docx`);
};

export const exportBangKhauHaoWord = async (assets: any[], selectedMonth: string, config?: CauHinhHeThong) => {
  const [year, month] = selectedMonth.split('-');
  
  const today = new Date();
  const day = today.getDate().toString().padStart(2, '0');
  const currentMonth = (today.getMonth() + 1).toString().padStart(2, '0');
  const currentYear = today.getFullYear();

  const noBorders = {
    top: { style: BorderStyle.NONE, size: 0, color: "auto" },
    bottom: { style: BorderStyle.NONE, size: 0, color: "auto" },
    left: { style: BorderStyle.NONE, size: 0, color: "auto" },
    right: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideHorizontal: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideVertical: { style: BorderStyle.NONE, size: 0, color: "auto" },
  };

  const doc = new Document({
    sections: [{
      properties: {
        page: {
          margin: { top: 1000, right: 1000, bottom: 1000, left: 1000 },
          size: { orientation: PageOrientation.LANDSCAPE }, 
        },
      },
      children: [
        // HEADER: Đơn vị / Địa chỉ
        new Paragraph({
          children: [
            new TextRun({ text: "Đơn vị: ", bold: true }),
            new TextRun({ text: config?.tenCongTy || "..........................................." })
          ]
        }),
        new Paragraph({
          children: [
            new TextRun({ text: "Mã số thuế: ", bold: true }),
            new TextRun({ text: config?.maSoThue || "..................." })
          ]
        }),
        new Paragraph({
          children: [
            new TextRun({ text: "Địa chỉ: ", bold: true }),
            new TextRun({ text: config?.diaChi || ".........................................." })
          ]
        }),

        new Paragraph({ text: "", spacing: { after: 300 } }),

        // TÊN TIÊU ĐỀ
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: "BẢNG TRÍCH KHẤU HAO TÀI SẢN CỐ ĐỊNH", bold: true, size: 32 })],
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: `Tháng ${month} năm ${year}`, italics: true })],
          spacing: { after: 400 }
        }),

        // BẢNG DỮ LIỆU CHÍNH
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          rows: [
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Mã TSCĐ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Tên TSCĐ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Ngày sử dụng", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Nguyên giá", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Hao mòn trong kỳ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Hao mòn lũy kế", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Giá trị còn lại", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
              ],
            }),
            ...assets.map(asset => new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: asset.code || "" })] }),
                new TableCell({ children: [new Paragraph({ text: asset.name || "" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: asset.ngayCapPhatStr || "" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.originalValue) })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.monthlyDepreciation) })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.accumulatedDepreciation) })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: formatVND(asset.remainingValue) })] }),
              ]
            })),
            new TableRow({
              children: [
                new TableCell({ columnSpan: 3, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Tổng cộng", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(assets.reduce((sum: number, a: any) => sum + (a.originalValue || 0), 0)), bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(assets.reduce((sum: number, a: any) => sum + (a.monthlyDepreciation || 0), 0)), bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(assets.reduce((sum: number, a: any) => sum + (a.accumulatedDepreciation || 0), 0)), bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(assets.reduce((sum: number, a: any) => sum + (a.remainingValue || 0), 0)), bold: true })] })] }),
              ]
            })
          ],
        }),

        // CHỮ KÝ FOOTER
        new Paragraph({
          alignment: AlignmentType.RIGHT,
          spacing: { before: 400, after: 100 },
          children: [new TextRun({ text: `Ngày ${day} tháng ${currentMonth} năm ${currentYear}`, italics: true })]
        }),

        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người lập", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Kế toán trưởng", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 34, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Giám đốc", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên, đóng dấu)", italics: true })] }),
                  ],
                }),
              ],
            }),
          ],
        }),
      ],
    }],
  });

  const buffer = await Packer.toBlob(doc);
  saveAs(buffer, `Bang_Trich_Khau_Hao_Thang_${month}_${year}.docx`);
};


// ─────────────────────────────────────────────────────────────────────────────
// exportSoCaiWord — v2 (fixed)
//
// Lỗi v1 đã sửa:
//  1. Header 3 row (rowSpan:3) kết hợp row index A-G → Word vỡ bảng.
//     → Đổi sang 2-row header (rowSpan:2), bỏ row A-G.
//  2. WidthType.PERCENTAGE không ổn định.
//     → Đổi sang WidthType.DXA với columnWidths khớp chính xác.
//  3. Dòng dữ liệu thiếu cell "Ngày CT" (cột 3).
//     → Đã thêm đủ 7 cell, đúng thứ tự.
//  4. "Tên tài khoản" / "Số hiệu" chưa căn giữa.
//     → Thêm AlignmentType.CENTER.
// ─────────────────────────────────────────────────────────────────────────────

export const exportSoCaiWord = async (
  data: any,
  accountCode: string,
  accountName: string,
  fromDate: string,
  toDate: string,
  config?: CauHinhHeThong
) => {
  const fromYear = new Date(fromDate).getFullYear();
  const toYear   = new Date(toDate).getFullYear();
  const displayYear = fromYear === toYear
    ? fromYear.toString()
    : `${fromYear} - ${toYear}`;

  const today        = new Date();
  const day          = today.getDate().toString().padStart(2, '0');
  const currentMonth = (today.getMonth() + 1).toString().padStart(2, '0');
  const currentYear  = today.getFullYear();

  // ── Borders ──────────────────────────────────────────────────────────────
  const noBorders = {
    top:              { style: BorderStyle.NONE, size: 0, color: "auto" },
    bottom:           { style: BorderStyle.NONE, size: 0, color: "auto" },
    left:             { style: BorderStyle.NONE, size: 0, color: "auto" },
    right:            { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideHorizontal: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideVertical:   { style: BorderStyle.NONE, size: 0, color: "auto" },
  };

  // ── Column widths (DXA) ───────────────────────────────────────────────────
  // A4 landscape content width = 16838 - 1000(L) - 1000(R) = 14838 DXA
  // 9 cột: NgàyGhiSổ | SốHiệuCT | NgàyCT | DiễnGiải | TrangSổNKC | STTDòngNKC | TKĐốiỨng | Nợ | Có
  const W = [1500, 1100, 1100, 2700, 900, 900, 1500, 2569, 2569] as const;
  // sum = 14838
  const TABLE_W = W.reduce((a, b) => a + b, 0);

  // ── Helper: tạo TableCell với width tự tính khi colSpan > 1 ──────────────
  const mkCell = (
    colIdx: number,
    children: Paragraph[],
    opts: { rowSpan?: number; columnSpan?: number; vAlign?: "center" | "top" | "bottom" } = {}
  ) => {
    const colSpan = opts.columnSpan ?? 1;
    const w = colSpan > 1
      ? (W as readonly number[]).slice(colIdx, colIdx + colSpan).reduce((a, b) => a + b, 0)
      : W[colIdx];
    return new TableCell({
      width: { size: w, type: WidthType.DXA },
      ...(opts.rowSpan  ? { rowSpan: opts.rowSpan }           : {}),
      ...(colSpan > 1   ? { columnSpan: colSpan }             : {}),
      ...(opts.vAlign   ? { verticalAlign: opts.vAlign as any } : {}),
      children,
    });
  };

  const hdr = (line1: string, line2?: string) =>
    new Paragraph({
      alignment: AlignmentType.CENTER,
      children: [
        new TextRun({ text: line1, bold: true }),
        ...(line2 ? [new TextRun({ text: line2, bold: true, break: 1 })] : []),
      ],
    });

  const idx = (label: string) =>
    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: label, bold: true })] });

  const empty = () => new Paragraph({ text: "" });

  // ── Dòng tổng hợp (7 cell trống đầu + 2 cell số) ─────────────────────────
  const summaryRow = (label: string, no: string, co: string) =>
    new TableRow({
      children: [
        mkCell(0, [empty()]),
        mkCell(1, [empty()]),
        mkCell(2, [empty()]),
        mkCell(3, [new Paragraph({ children: [new TextRun({ text: label, bold: true })] })]),
        mkCell(4, [empty()]),
        mkCell(5, [empty()]),
        mkCell(6, [empty()]),
        mkCell(7, [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: no, bold: true })] })]),
        mkCell(8, [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: co, bold: true })] })]),
      ],
    });

  // ── Document ──────────────────────────────────────────────────────────────
  const doc = new Document({
    sections: [{
      properties: {
        page: {
          margin: { top: 1000, right: 1000, bottom: 1000, left: 1000 },
          size: { orientation: PageOrientation.LANDSCAPE },
        },
      },
      children: [

        // ── HEADER (đơn vị / mẫu số) ───────────────────────────────────────
        new Table({
          width: { size: TABLE_W, type: WidthType.DXA },
          columnWidths: [Math.round(TABLE_W * 0.55), TABLE_W - Math.round(TABLE_W * 0.55)],
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: Math.round(TABLE_W * 0.55), type: WidthType.DXA },
                  borders: noBorders,
                  children: [
                    new Paragraph({ children: [new TextRun({ text: "Đơn vị: ", bold: true }), new TextRun({ text: config?.tenCongTy || "..........................................." })] }),
                    new Paragraph({ children: [new TextRun({ text: "Mã số thuế: ", bold: true }), new TextRun({ text: config?.maSoThue || "..................." })] }),
                    new Paragraph({ children: [new TextRun({ text: "Địa chỉ: ", bold: true }), new TextRun({ text: config?.diaChi || ".........................................." })] }),
                  ],
                }),
                new TableCell({
                  width: { size: TABLE_W - Math.round(TABLE_W * 0.55), type: WidthType.DXA },
                  borders: noBorders,
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Mẫu số S03b-DN", bold: true })] }),
                    new Paragraph({
                      alignment: AlignmentType.CENTER,
                      children: [
                        new TextRun({ text: "(Kèm theo Thông tư số 99/2025/TT-BTC", italics: true, size: 20 }),
                        new TextRun({ text: "ngày 27 tháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)", italics: true, size: 20, break: 1 }),
                      ],
                    }),
                  ],
                }),
              ],
            }),
          ],
        }),

        new Paragraph({ text: "", spacing: { after: 200 } }),

        // ── TIÊU ĐỀ ────────────────────────────────────────────────────────
        new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "SỔ CÁI", bold: true, size: 32 })] }),
        new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Dùng cho hình thức kế toán Nhật ký chung)", italics: true })] }),
        new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: `Năm: ${displayYear}`, bold: true })], spacing: { after: 100 } }),
        new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: `Tên tài khoản: ${accountName}` })] }),
        new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: `Số hiệu: ${accountCode}` })], spacing: { after: 200 } }),

        // ── BẢNG SỔ CÁI ────────────────────────────────────────────────────
        // Cấu trúc 3-row header — trick: rowSpan:2 (không phải 3)
        // → row 3 (index A-H) được tự do có đủ 9 cell độc lập
        //
        // Cột: 0=NgàyGhiSổ | 1=SốHiệuCT | 2=NgàyCT | 3=DiễnGiải
        //       4=TrangSổNKC | 5=STTDòngNKC | 6=TKĐốiỨng | 7=Nợ | 8=Có
        new Table({
          width: { size: TABLE_W, type: WidthType.DXA },
          columnWidths: [...W],
          rows: [

            // ── Header row 1: nhóm cột (rowSpan:2 → row 3 tự do) ──────────
            new TableRow({
              children: [
                mkCell(0, [hdr("Ngày, tháng", "ghi sổ")],   { rowSpan: 2, vAlign: "center" }),
                mkCell(1, [hdr("Chứng từ")],                 { columnSpan: 2, vAlign: "center" }),
                mkCell(3, [hdr("Diễn giải")],                { rowSpan: 2, vAlign: "center" }),
                mkCell(4, [hdr("Nhật ký chung")],            { columnSpan: 2, vAlign: "center" }),
                mkCell(6, [hdr("Số hiệu TK", "đối ứng")],   { rowSpan: 2, vAlign: "center" }),
                mkCell(7, [hdr("Số tiền")],                  { columnSpan: 2, vAlign: "center" }),
              ],
            }),

            // ── Header row 2: nhãn con (chỉ 6 cell — bỏ qua vị trí rowSpan) ─
            new TableRow({
              children: [
                mkCell(1, [hdr("Số hiệu")]),
                mkCell(2, [hdr("Ngày, tháng")]),
                mkCell(4, [hdr("Trang sổ")]),
                mkCell(5, [hdr("STT dòng")]),
                mkCell(7, [hdr("Nợ")]),
                mkCell(8, [hdr("Có")]),
              ],
            }),

            // ── Header row 3: index A-H (đủ 9 cell — hợp lệ vì rowSpan đã hết) ─
            new TableRow({
              children: [
                mkCell(0, [idx("A")]),
                mkCell(1, [idx("B")]),
                mkCell(2, [idx("C")]),
                mkCell(3, [idx("D")]),
                mkCell(4, [idx("E")]),
                mkCell(5, [idx("F")]),
                mkCell(6, [idx("G")]),
                mkCell(7, [idx("H")]),
                mkCell(8, [idx("1")]),
              ],
            }),

            // ── Số dư đầu kỳ ──────────────────────────────────────────────
            summaryRow(
              "- Số dư đầu kỳ",
              data.soDuDauKy >= 0 ? formatVND(data.soDuDauKy) : "",
              data.soDuDauKy < 0  ? formatVND(Math.abs(data.soDuDauKy)) : ""
            ),

            // ── Dữ liệu phát sinh (9 cell đúng thứ tự) ────────────────────
            ...(data.butToans || []).map((b: any) =>
              new TableRow({
                children: [
                  // 0: Ngày ghi sổ
                  mkCell(0, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.ngayHachToan ? new Date(b.ngayHachToan).toLocaleDateString('vi-VN') : "" })]),
                  // 1: Số hiệu chứng từ
                  mkCell(1, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.maChungTu || "" })]),
                  // 2: Ngày, tháng chứng từ
                  mkCell(2, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.ngayHachToan ? new Date(b.ngayHachToan).toLocaleDateString('vi-VN') : "" })]),
                  // 3: Diễn giải
                  mkCell(3, [new Paragraph({ text: b.dienGiai || "" })]),
                  // 4: Trang sổ NKC (để trống — không có trong data)
                  mkCell(4, [new Paragraph({ text: "" })]),
                  // 5: STT dòng NKC (để trống — không có trong data)
                  mkCell(5, [new Paragraph({ text: "" })]),
                  // 6: TK đối ứng
                  mkCell(6, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.taiKhoanDoiUng || "" })]),
                  // 7: Phát sinh Nợ
                  mkCell(7, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.phatSinhNo > 0 ? formatVND(b.phatSinhNo) : "" })]),
                  // 8: Phát sinh Có
                  mkCell(8, [new Paragraph({ alignment: AlignmentType.CENTER, text: b.phatSinhCo > 0 ? formatVND(b.phatSinhCo) : "" })]),
                ],
              })
            ),

            // ── Cộng số phát sinh ──────────────────────────────────────────
            summaryRow(
              "- Cộng số phát sinh",
              formatVND(data.phatSinhNo),
              formatVND(data.phatSinhCo)
            ),

            // ── Số dư cuối kỳ ──────────────────────────────────────────────
            summaryRow(
              "- Số dư cuối kỳ",
              data.soDuCuoiKy >= 0 ? formatVND(data.soDuCuoiKy) : "",
              data.soDuCuoiKy < 0  ? formatVND(Math.abs(data.soDuCuoiKy)) : ""
            ),
          ],
        }),

        // ── FOOTER ─────────────────────────────────────────────────────────
        new Paragraph({ spacing: { before: 200 }, children: [new TextRun("Sổ này có .... trang, đánh số từ trang số 01 đến trang ....")] }),
        new Paragraph({ spacing: { before: 100 }, children: [new TextRun("Ngày mở sổ:................................................................")] }),

        new Paragraph({
          alignment: AlignmentType.RIGHT,
          spacing: { before: 200, after: 100 },
          children: [new TextRun({ text: `Ngày ${day} tháng ${currentMonth} năm ${currentYear}`, italics: true })],
        }),

        new Table({
          width: { size: TABLE_W, type: WidthType.DXA },
          columnWidths: [Math.floor(TABLE_W / 3), Math.floor(TABLE_W / 3), TABLE_W - 2 * Math.floor(TABLE_W / 3)],
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: Math.floor(TABLE_W / 3), type: WidthType.DXA },
                  borders: noBorders,
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người ghi sổ", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: Math.floor(TABLE_W / 3), type: WidthType.DXA },
                  borders: noBorders,
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Kế toán trưởng", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: TABLE_W - 2 * Math.floor(TABLE_W / 3), type: WidthType.DXA },
                  borders: noBorders,
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người đại diện theo pháp luật", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên, đóng dấu)", italics: true })] }),
                  ],
                }),
              ],
            }),
          ],
        }),

      ],
    }],
  });

  const buffer = await Packer.toBlob(doc);
  saveAs(buffer, `So_Cai_${accountCode}_${fromDate}_${toDate}.docx`);
};

export const exportNhatKyChungWord = async (
  entries: any[],
  fromDate: string,
  toDate: string,
  config?: CauHinhHeThong
) => {
  const today = new Date();
  const day = today.getDate().toString().padStart(2, '0');
  const currentMonth = (today.getMonth() + 1).toString().padStart(2, '0');
  const currentYear = today.getFullYear();

  const fromYear = new Date(fromDate).getFullYear();
  const toYear = new Date(toDate).getFullYear();
  const displayYear = fromYear === toYear ? fromYear.toString() : `${fromYear} - ${toYear}`;

  const noBorders = {
    top: { style: BorderStyle.NONE, size: 0, color: "auto" },
    bottom: { style: BorderStyle.NONE, size: 0, color: "auto" },
    left: { style: BorderStyle.NONE, size: 0, color: "auto" },
    right: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideHorizontal: { style: BorderStyle.NONE, size: 0, color: "auto" },
    insideVertical: { style: BorderStyle.NONE, size: 0, color: "auto" },
  };

  let totalNo = 0;
  let totalCo = 0;
  entries.forEach(e => {
    if (e.taiKhoanNo) totalNo += (e.soTien || 0);
    if (e.taiKhoanCo) totalCo += (e.soTien || 0);
  });

  const doc = new Document({
    sections: [{
      properties: {
        page: {
          margin: { top: 1000, right: 1000, bottom: 1000, left: 1000 },
          size: { orientation: PageOrientation.LANDSCAPE },
        },
      },
      children: [
        // HEADER
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: 55, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Đơn vị: ", bold: true }),
                        new TextRun({ text: config?.tenCongTy || "..........................................." })
                      ]
                    }),
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Mã số thuế: ", bold: true }),
                        new TextRun({ text: config?.maSoThue || "..................." })
                      ]
                    }),
                    new Paragraph({
                      children: [
                        new TextRun({ text: "Địa chỉ: ", bold: true }),
                        new TextRun({ text: config?.diaChi || ".........................................." })
                      ]
                    }),
                  ],
                }),
                new TableCell({
                  width: { size: 45, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Mẫu số S03a-DN", bold: true })] }),
                    new Paragraph({
                      alignment: AlignmentType.CENTER,
                      children: [
                        new TextRun({ text: "(Kèm theo Thông tư số 99/2025/TT-BTC", italics: true, size: 20 }),
                        new TextRun({ text: "ngày 27 tháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)", italics: true, size: 20, break: 1 })
                      ]
                    }),
                  ],
                }),
              ],
            }),
          ],
        }),

        new Paragraph({ text: "", spacing: { after: 200 } }),

        // TÊN TIÊU ĐỀ
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: "SỔ NHẬT KÝ CHUNG", bold: true, size: 32 })],
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: `Năm: ${displayYear}`, bold: true })],
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun({ text: `Từ ngày ${new Date(fromDate).toLocaleDateString('vi-VN')} đến ngày ${new Date(toDate).toLocaleDateString('vi-VN')}`, italics: true })],
          spacing: { after: 200 }
        }),

        new Paragraph({
          alignment: AlignmentType.RIGHT,
          children: [new TextRun({ text: "Đơn vị tính: VNĐ", italics: true })],
          spacing: { after: 100 }
        }),

        // BẢNG SỔ NHẬT KÝ CHUNG
        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          rows: [
            new TableRow({
              children: [
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Ngày, tháng\nghi sổ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ columnSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Chứng từ", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Diễn giải", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Đã ghi\nSổ Cái", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "STT\ndòng", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ rowSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số hiệu TK\nđối ứng", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
                new TableCell({ columnSpan: 2, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số phát sinh", bold: true })] })], verticalAlign: VerticalAlign.CENTER }),
              ],
            }),
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Số\nhiệu", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Ngày,\ntháng", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Nợ", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Có", bold: true })] })] }),
              ],
            }),
            new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "A" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "B" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "C" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "D" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "E" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "G" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "H" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "1" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: "2" })] }),
              ],
            }),

            ...entries.map(e => new TableRow({
              children: [
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: e.ngayGhiSo ? new Date(e.ngayGhiSo).toLocaleDateString('vi-VN') : "" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: e.maChungTu || "" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: e.ngayLap ? new Date(e.ngayLap).toLocaleDateString('vi-VN') : "" })] }),
                new TableCell({ children: [new Paragraph({ text: e.dienGiai || "" })] }),
                new TableCell({ children: [new Paragraph({ text: "" })] }),
                new TableCell({ children: [new Paragraph({ text: "" })] }), 
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.CENTER, text: [e.taiKhoanNo, e.taiKhoanCo].filter(Boolean).join(' - ') })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: e.taiKhoanNo ? formatVND(e.soTien) : "" })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, text: e.taiKhoanCo ? formatVND(e.soTien) : "" })] }),
              ],
            })),

            new TableRow({
              children: [
                new TableCell({ columnSpan: 7, children: [new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Cộng số phát sinh", bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(totalNo), bold: true })] })] }),
                new TableCell({ children: [new Paragraph({ alignment: AlignmentType.RIGHT, children: [new TextRun({ text: formatVND(totalCo), bold: true })] })] }),
              ],
            }),
          ],
        }),

        // FOOTER & CHỮ KÝ
        new Paragraph({ spacing: { before: 200 }, children: [new TextRun("- Sổ này có .... trang, đánh số từ trang số 01 đến trang ....")] }),
        new Paragraph({ spacing: { before: 100 }, children: [new TextRun("- Ngày mở sổ:................................................................")] }),
        
        new Paragraph({
          alignment: AlignmentType.RIGHT,
          spacing: { before: 200, after: 100 },
          children: [new TextRun({ text: `Ngày ${day} tháng ${currentMonth} năm ${currentYear}`, italics: true })]
        }),

        new Table({
          width: { size: 100, type: WidthType.PERCENTAGE },
          borders: noBorders,
          rows: [
            new TableRow({
              children: [
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người ghi sổ", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 33, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Kế toán trưởng", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên)", italics: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 34, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Người đại diện theo pháp luật", bold: true })] }),
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "(Ký, họ tên, đóng dấu)", italics: true })] }),
                  ],
                }),
              ],
            }),
          ],
        }),
      ],
    }],
  });

  const buffer = await Packer.toBlob(doc);
  saveAs(buffer, `So_Nhat_Ky_Chung_${fromDate}_${toDate}.docx`);
};