import { useState, useEffect } from 'react';
import { BookOpen, RefreshCw, Download, TrendingUp, TrendingDown, Scale } from 'lucide-react';
import { toast } from 'sonner';
import * as XLSX from 'xlsx';
import { generalLedgerApi, SoCaiChiTietResponse } from '../../api/generalLedgerApi';

const ACCOUNTS = [
  { ma: '211', ten: 'Tài sản cố định hữu hình' },
  { ma: '214', ten: 'Hao mòn tài sản cố định' },
];

const fmt = (n: number) =>
  n === 0 ? '—' : n.toLocaleString('vi-VN', { minimumFractionDigits: 0, maximumFractionDigits: 0 });

const fmtFull = (n: number) =>
  n.toLocaleString('vi-VN', { minimumFractionDigits: 0, maximumFractionDigits: 0 });

const fmtDate = (d?: string | null) =>
  d ? new Date(d).toLocaleDateString('vi-VN') : '—';

const today = new Date();
const firstOfYear = `${today.getFullYear()}-01-01`;
const lastOfYear = `${today.getFullYear()}-12-31`;

export function GeneralLedger() {
  const [selectedMa, setSelectedMa] = useState(ACCOUNTS[0].ma);
  const [fromDate, setFromDate] = useState(firstOfYear);
  const [toDate, setToDate] = useState(lastOfYear);
  const [data, setData] = useState<SoCaiChiTietResponse | null>(null);
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const res = await generalLedgerApi.getChiTiet(selectedMa, fromDate, toDate);
      if (res.errorCode === 200) setData(res.data);
      else toast.error(res.message ?? 'Lỗi tải dữ liệu');
    } catch {
      toast.error('Không thể kết nối đến máy chủ');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, [selectedMa, fromDate, toDate]);

  const selectedAccount = ACCOUNTS.find(a => a.ma === selectedMa)!;

  const exportExcel = () => {
    if (!data) return;
    const rows: any[] = [];
    rows.push({
      'Ngày hạch toán': '',
      'Số chứng từ': '',
      'Diễn giải': `Số dư đầu kỳ`,
      'Phát sinh Nợ': '',
      'Phát sinh Có': '',
      'Số dư lũy kế': data.soDuDauKy,
    });
    data.butToans.forEach(b => {
      rows.push({
        'Ngày hạch toán': fmtDate(b.ngayHachToan),
        'Số chứng từ': b.maChungTu ?? '',
        'Diễn giải': b.dienGiai ?? '',
        'Phát sinh Nợ': b.phatSinhNo || '',
        'Phát sinh Có': b.phatSinhCo || '',
        'Số dư lũy kế': b.soDuLuyKe,
      });
    });
    rows.push({
      'Ngày hạch toán': '',
      'Số chứng từ': '',
      'Diễn giải': 'Tổng cộng',
      'Phát sinh Nợ': data.phatSinhNo,
      'Phát sinh Có': data.phatSinhCo,
      'Số dư lũy kế': data.soDuCuoiKy,
    });
    const ws = XLSX.utils.json_to_sheet(rows);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, `Sổ cái ${selectedMa}`);
    XLSX.writeFile(wb, `So_cai_${selectedMa}_${fromDate}_${toDate}.xlsx`);
  };

  const soDuCuoiKy = data?.soDuCuoiKy ?? 0;

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-blue-600 rounded-lg">
              <BookOpen className="w-5 h-5 text-white" />
            </div>
            <div>
              <h1 className="text-xl font-bold text-gray-900">Sổ Cái</h1>
              <p className="text-xs text-gray-500">Mẫu S03b-DN — Theo Thông tư 99/2025/TT-BTC</p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <button
              onClick={load}
              disabled={loading}
              className="flex items-center gap-2 px-3 py-2 border border-gray-300 rounded-lg text-sm hover:bg-gray-50 disabled:opacity-50"
            >
              <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
              Làm mới
            </button>
            <button
              onClick={exportExcel}
              disabled={!data}
              className="flex items-center gap-2 px-3 py-2 bg-green-600 text-white rounded-lg text-sm hover:bg-green-700 disabled:opacity-50"
            >
              <Download className="w-4 h-4" />
              Xuất Excel
            </button>
          </div>
        </div>
      </div>

      <div className="max-w-6xl mx-auto px-6 py-6 space-y-5">
        {/* Filters */}
        <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-4">
          <div className="flex flex-wrap items-end gap-4">
            <div className="flex-1 min-w-[200px]">
              <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1.5">
                Tài khoản
              </label>
              <select
                value={selectedMa}
                onChange={e => setSelectedMa(e.target.value)}
                className="w-full px-3 py-2.5 border border-gray-300 rounded-lg text-sm font-medium focus:ring-2 focus:ring-blue-500 focus:outline-none bg-white"
              >
                {ACCOUNTS.map(a => (
                  <option key={a.ma} value={a.ma}>
                    {a.ma} — {a.ten}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1.5">
                Từ ngày
              </label>
              <input
                type="date"
                value={fromDate}
                onChange={e => setFromDate(e.target.value)}
                className="px-3 py-2.5 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-blue-500 focus:outline-none"
              />
            </div>
            <div>
              <label className="block text-xs font-semibold text-gray-500 uppercase tracking-wide mb-1.5">
                Đến ngày
              </label>
              <input
                type="date"
                value={toDate}
                onChange={e => setToDate(e.target.value)}
                className="px-3 py-2.5 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-blue-500 focus:outline-none"
              />
            </div>
          </div>
        </div>

        {/* Account info + Stat cards */}
        {data && (
          <>
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
              <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-4 flex items-center gap-4">
                <div className="p-3 bg-blue-50 rounded-lg">
                  <TrendingUp className="w-5 h-5 text-blue-600" />
                </div>
                <div>
                  <p className="text-xs text-gray-500 font-medium">Tổng phát sinh Nợ</p>
                  <p className="text-xl font-bold text-blue-700">{fmtFull(data.phatSinhNo)}</p>
                </div>
              </div>
              <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-4 flex items-center gap-4">
                <div className="p-3 bg-red-50 rounded-lg">
                  <TrendingDown className="w-5 h-5 text-red-500" />
                </div>
                <div>
                  <p className="text-xs text-gray-500 font-medium">Tổng phát sinh Có</p>
                  <p className="text-xl font-bold text-red-600">{fmtFull(data.phatSinhCo)}</p>
                </div>
              </div>
              <div className={`bg-white rounded-xl border shadow-sm p-4 flex items-center gap-4 ${soDuCuoiKy >= 0 ? 'border-blue-200' : 'border-red-200'}`}>
                <div className={`p-3 rounded-lg ${soDuCuoiKy >= 0 ? 'bg-blue-50' : 'bg-red-50'}`}>
                  <Scale className={`w-5 h-5 ${soDuCuoiKy >= 0 ? 'text-blue-600' : 'text-red-500'}`} />
                </div>
                <div>
                  <p className="text-xs text-gray-500 font-medium">Số dư cuối kỳ</p>
                  <p className={`text-xl font-bold ${soDuCuoiKy >= 0 ? 'text-blue-700' : 'text-red-600'}`}>
                    {fmtFull(Math.abs(soDuCuoiKy))}
                    {soDuCuoiKy < 0 && <span className="text-sm font-normal ml-1">(Có)</span>}
                  </p>
                </div>
              </div>
            </div>

            {/* Account info banner */}
            <div className="bg-blue-50 border border-blue-100 rounded-xl px-5 py-3 flex items-center gap-4">
              <div className="w-10 h-10 rounded-full bg-blue-600 flex items-center justify-center shrink-0">
                <span className="text-white font-bold text-sm">{selectedAccount.ma}</span>
              </div>
              <div className="flex-1">
                <p className="font-semibold text-blue-900">{data.tenTaiKhoan ?? selectedAccount.ten}</p>
                <p className="text-xs text-blue-600">Loại: {data.loaiTaiKhoan ?? '—'} · Số dư đầu kỳ: <strong>{fmtFull(data.soDuDauKy)}</strong></p>
              </div>
              <div className="text-right">
                <p className="text-xs text-blue-500">Kỳ kế toán</p>
                <p className="text-sm font-semibold text-blue-900">
                  {new Date(fromDate).toLocaleDateString('vi-VN')} – {new Date(toDate).toLocaleDateString('vi-VN')}
                </p>
              </div>
            </div>

            {/* Ledger table */}
            <div className="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden">
              <div className="px-4 py-3 border-b border-gray-100 bg-gray-50">
                <p className="text-sm font-semibold text-gray-700">
                  TÀI KHOẢN {selectedAccount.ma} — {data.tenTaiKhoan ?? selectedAccount.ten}
                </p>
                <p className="text-xs text-gray-400">
                  Từ {new Date(fromDate).toLocaleDateString('vi-VN')} đến {new Date(toDate).toLocaleDateString('vi-VN')}
                </p>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="bg-gray-50 border-b border-gray-200 text-gray-600 text-xs font-semibold uppercase">
                      <th className="px-4 py-3 text-left w-32">Ngày hạch toán</th>
                      <th className="px-4 py-3 text-left w-36">Số chứng từ</th>
                      <th className="px-4 py-3 text-left">Diễn giải</th>
                      <th className="px-4 py-3 text-right w-36">Phát sinh Nợ</th>
                      <th className="px-4 py-3 text-right w-36">Phát sinh Có</th>
                      <th className="px-4 py-3 text-right w-36">Số dư lũy kế</th>
                    </tr>
                  </thead>
                  <tbody>
                    {/* Opening balance row */}
                    <tr className="bg-yellow-50 border-b border-yellow-100 font-semibold">
                      <td className="px-4 py-3 text-gray-500 text-xs">—</td>
                      <td className="px-4 py-3 text-gray-500 text-xs">—</td>
                      <td className="px-4 py-3 text-gray-700">Số dư đầu kỳ</td>
                      <td className="px-4 py-3 text-right text-gray-400">—</td>
                      <td className="px-4 py-3 text-right text-gray-400">—</td>
                      <td className="px-4 py-3 text-right text-blue-700 font-bold">{fmtFull(data.soDuDauKy)}</td>
                    </tr>

                    {/* Transaction rows */}
                    {data.butToans.length === 0 ? (
                      <tr>
                        <td colSpan={6} className="px-4 py-8 text-center text-gray-400">
                          Không có phát sinh trong kỳ
                        </td>
                      </tr>
                    ) : (
                      data.butToans.map((b, idx) => (
                        <tr key={idx} className="border-b border-gray-100 hover:bg-gray-50 transition-colors">
                          <td className="px-4 py-3 text-gray-600 text-xs">{fmtDate(b.ngayHachToan)}</td>
                          <td className="px-4 py-3">
                            <span className="font-medium text-blue-600 text-xs">{b.maChungTu ?? '—'}</span>
                          </td>
                          <td className="px-4 py-3 text-gray-700 text-xs truncate max-w-[260px]" title={b.dienGiai ?? ''}>
                            {b.dienGiai ?? '—'}
                          </td>
                          <td className="px-4 py-3 text-right">
                            {b.phatSinhNo > 0
                              ? <span className="font-medium text-blue-700">{fmt(b.phatSinhNo)}</span>
                              : <span className="text-gray-300">—</span>}
                          </td>
                          <td className="px-4 py-3 text-right">
                            {b.phatSinhCo > 0
                              ? <span className="font-medium text-red-600">{fmt(b.phatSinhCo)}</span>
                              : <span className="text-gray-300">—</span>}
                          </td>
                          <td className="px-4 py-3 text-right">
                            <span className={`font-semibold ${b.soDuLuyKe >= 0 ? 'text-blue-700' : 'text-red-600'}`}>
                              {fmtFull(b.soDuLuyKe)}
                            </span>
                          </td>
                        </tr>
                      ))
                    )}

                    {/* Total row */}
                    <tr className="bg-gray-50 border-t-2 border-gray-200 font-bold">
                      <td className="px-4 py-3" colSpan={3}>
                        <span className="text-gray-700 text-sm">Tổng cộng phát sinh</span>
                      </td>
                      <td className="px-4 py-3 text-right text-blue-700">{fmtFull(data.phatSinhNo)}</td>
                      <td className="px-4 py-3 text-right text-red-600">{fmtFull(data.phatSinhCo)}</td>
                      <td className="px-4 py-3 text-right text-gray-900">—</td>
                    </tr>
                    <tr className="bg-blue-50 border-t border-blue-100 font-bold">
                      <td className="px-4 py-3" colSpan={3}>
                        <span className="text-blue-800 text-sm">Số dư cuối kỳ</span>
                      </td>
                      <td className="px-4 py-3 text-right text-gray-400">—</td>
                      <td className="px-4 py-3 text-right text-gray-400">—</td>
                      <td className="px-4 py-3 text-right text-blue-800 text-base">
                        {fmtFull(data.soDuCuoiKy)}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </>
        )}

        {/* Loading state */}
        {loading && (
          <div className="bg-white rounded-xl border border-gray-200 p-12 text-center text-gray-500">
            <RefreshCw className="w-8 h-8 animate-spin mx-auto mb-3 text-blue-500" />
            <p>Đang tải dữ liệu sổ cái...</p>
          </div>
        )}

        {/* Empty state */}
        {!loading && !data && (
          <div className="bg-white rounded-xl border border-gray-200 p-12 text-center text-gray-400">
            <BookOpen className="w-12 h-12 mx-auto mb-3 text-gray-200" />
            <p className="font-medium">Không có dữ liệu cho tài khoản và kỳ đã chọn.</p>
          </div>
        )}
      </div>
    </div>
  );
}
