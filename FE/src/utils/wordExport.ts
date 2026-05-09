import {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  WidthType, AlignmentType, BorderStyle, VerticalAlign
} from "docx";
import { saveAs } from "file-saver";
import { TaiSan } from "../api/assetApi";
import { Department } from "../api/departmentApi";
import { AssetCategory } from "../api/assetCategoryApi";

// Hàm hỗ trợ format tiền
const formatVND = (value?: number) => {
  if (value === undefined) return "";
  return new Intl.NumberFormat('vi-VN').format(value);
};

export const exportTheTSCDWord = async (
  asset: TaiSan,
  categories: AssetCategory[],
  departments: Department[]
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
                  width: { size: 40, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ children: [new TextRun({ text: "Đơn vị: ...........................................", bold: true })] }),
                    new Paragraph({ children: [new TextRun({ text: "Địa chỉ: ..........................................", bold: true })] }),
                  ],
                }),
                new TableCell({
                  width: { size: 60, type: WidthType.PERCENTAGE },
                  children: [
                    new Paragraph({ alignment: AlignmentType.CENTER, children: [new TextRun({ text: "Mẫu số S23-DN", bold: true })] }),
                    new Paragraph({
                      alignment: AlignmentType.CENTER,
                      children: [new TextRun({ text: "(Kèm theo Thông tư số 99/2025/TT-BTC\nngày 27 tháng 10 năm 2025 của Bộ trưởng Bộ Tài chính)", italics: true, size: 20 })]
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
            // Dòng dữ liệu mẫu 1 (Có thể map() từ dữ liệu lịch sử nếu có)
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

        // Đã sửa thuộc tính 'bold' bằng cách chuyển văn bản vào trong TextRun
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

  // Xuất file
  const buffer = await Packer.toBlob(doc);
  saveAs(buffer, `The_TSCD_${asset.maTaiSan}.docx`);
};