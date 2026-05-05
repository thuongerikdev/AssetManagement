import { useState, useEffect } from 'react';
import { BookOpen, Search, ChevronRight, ArrowLeft, Download, RefreshCw } from 'lucide-react';
import { toast } from 'sonner';
import * as XLSX from 'xlsx';
import { generalLedgerApi, SoCaiTomTatResponse, SoCaiChiTietResponse } from '../../api/generalLedgerApi';

const fmt = (n: number) =>
  n.toLocaleString('vi-VN', { minimumFractionDigits: 0, maximumFractionDigits: 0 });

const fmtDate = (d?: string) =>
  d ? new Date(d).toLocaleDateString('vi-VN') : '';

function BalanceCell({ value }: { value: number }) {
  const color = value > 0 ? 'text-blue-700' : value < 0 ? 'text-red-600' : 'text-gray-500';
  return <span className={`font-medium ${color}`}>{fmt(value)}</span>;
}

// ============================
// VIEW: DANH SÁCH TÀI KHOẢN
// ============================
function AccountListView({
  fromDate,
  toDate,
  onSelectAccount,
}: {
  fromDate: string;
  toDate: string;
  onSelectAccount: (maTK: string) => void;
}) {
  const [rows, setRows] = useState<SoCaiTomTatResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      const res = await generalLedgerApi.getTomTat(fromDate, toDate);
      if (res.errorCode === 200) setRows(res.data ?? []);
      else toast.error(res.message ?? 'Lỗi tải dữ liệu');
    } catch {
      toast.error('Không thể kết nối đến máy chủ');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, [fromDate, toDate]);

  const filtered = rows.filter(r =>
    r.maTaiKhoan.toLowerCase().includes(search.toLowerCase()) ||
    (r.tenTaiKhoan ?? '').toLowerCase().includes(search.toLowerCase())
  );

  const exportExcel = () => {
    const ws = XLSX.utils.json_to_sheet(filtered.map(r => ({
      'Mã TK': r.maTaiKhoan,
      'Tên tài khoản': r.tenTaiKhoan ?? '',
      'Loại': r.loaiTaiKhoan ?? '',
      'Số dư đầu kỳ': r.soDuDauKy,
      'Phát sinh Nợ': r.phatSinhNo,
      'Phát sinh Có': r.phatSinhCo,
      'Số dư cuối kỳ': r.soDuCuoiKy,
      'Số bút toán': r.soLuongButToan,
    })));
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sổ cái tổng hợp');
    XLSX.writeFile(wb, `so-cai-tong-hop-${fromDate}-${toDate}.xlsx`);
  };

  const totalNo = filtered.reduce((s, r) => s + r.phatSinhNo, 0);
  const totalCo = filtered.reduce((s, r) => s + r.phatSinhCo, 0);

  return (
    <div className="space-y-4">
      {/* Toolbar */}
      <div className="flex flex-col sm:flex-row gap-3 items-start sm:items-center justify-between">
        <div className="relative flex-1 max-w-xs">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
          <input
            className="w-full pl-9 pr-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Tìm theo mã hoặc tên tài khoản..."
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>
        <div className="flex gap-2">
          <button
            onClick={load}
            disabled={loading}
            className="flex items-center gap-1.5 px-3 py-2 text-sm bg-gray-100 hover:bg-gray-200 rounded-lg border border-gray-300 transition-colors disabled:opacity-50"
          >
            <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
            Làm mới
          </button>
          <button
            onClick={exportExcel}
            disabled={filtered.length === 0}
            className="flex items-center gap-1.5 px-3 py-2 text-sm bg-green-600 hover:bg-green-700 text-white rounded-lg transition-colors disabled:opacity-50"
          >
            <Download className="w-4 h-4" />
            Xuất Excel
          </button>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
        {loading ? (
          <div className="flex justify-center items-center py-16 text-gray-500">
            <RefreshCw className="w-5 h-5 animate-spin mr-2" />
            Đang tải dữ liệu...
          </div>
        ) : filtered.length === 0 ? (
          <div className="text-center py-16 text-gray-400">
            <BookOpen className="w-10 h-10 mx-auto mb-3 opacity-40" />
            <p>Không có dữ liệu phát sinh trong kỳ</p>
          </div>
        ) : (
          <table className="w-full text-sm">
            <thead className="bg-blue-50 border-b border-blue-100">
              <tr>
                <th className="text-left px-4 py-3 font-semibold text-blue-800">Mã TK</th>
                <th className="text-left px-4 py-3 font-semibold text-blue-800">Tên tài khoản</th>
                <th className="text-left px-4 py-3 font-semibold text-blue-800">Loại</th>
                <th className="text-right px-4 py-3 font-semibold text-blue-800">Số dư đầu kỳ</th>
                <th className="text-right px-4 py-3 font-semibold text-blue-800">Phát sinh Nợ</th>
                <th className="text-right px-4 py-3 font-semibold text-blue-800">Phát sinh Có</th>
                <th className="text-right px-4 py-3 font-semibold text-blue-800">Số dư cuối kỳ</th>
                <th className="text-center px-4 py-3 font-semibold text-blue-800">Bút toán</th>
                <th className="px-4 py-3"></th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {filtered.map(r => (
                <tr
                  key={r.maTaiKhoan}
                  className="hover:bg-blue-50/50 transition-colors cursor-pointer"
                  onClick={() => onSelectAccount(r.maTaiKhoan)}
                >
                  <td className="px-4 py-3 font-mono font-medium text-blue-700">{r.maTaiKhoan}</td>
                  <td className="px-4 py-3 text-gray-800">{r.tenTaiKhoan ?? '—'}</td>
                  <td className="px-4 py-3">
                    {r.loaiTaiKhoan && (
                      <span className="px-2 py-0.5 rounded-full text-xs bg-gray-100 text-gray-600">
                        {r.loaiTaiKhoan}
                      </span>
                    )}
                  </td>
                  <td className="px-4 py-3 text-right"><BalanceCell value={r.soDuDauKy} /></td>
                  <td className="px-4 py-3 text-right font-medium text-gray-700">{fmt(r.phatSinhNo)}</td>
                  <td className="px-4 py-3 text-right font-medium text-gray-700">{fmt(r.phatSinhCo)}</td>
                  <td className="px-4 py-3 text-right"><BalanceCell value={r.soDuCuoiKy} /></td>
                  <td className="px-4 py-3 text-center text-gray-500">{r.soLuongButToan}</td>
                  <td className="px-4 py-3 text-gray-400">
                    <ChevronRight className="w-4 h-4" />
                  </td>
                </tr>
              ))}
            </tbody>
            <tfoot className="bg-gray-50 border-t-2 border-gray-200 font-semibold text-gray-800">
              <tr>
                <td colSpan={4} className="px-4 py-3">Tổng cộng ({filtered.length} tài khoản)</td>
                <td className="px-4 py-3 text-right text-blue-700">{fmt(totalNo)}</td>
                <td className="px-4 py-3 text-right text-red-600">{fmt(totalCo)}</td>
                <td colSpan={3}></td>
              </tr>
            </tfoot>
          </table>
        )}
      </div>
    </div>
  );
}

// ============================
// VIEW: CHI TIẾT TÀI KHOẢN
// ============================
function AccountDetailView({
  maTaiKhoan,
  fromDate,
  toDate,
  onBack,
}: {
  maTaiKhoan: string;
  fromDate: string;
  toDate: string;
  onBack: () => void;
}) {
  const [detail, setDetail] = useState<SoCaiChiTietResponse | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);
    generalLedgerApi
      .getChiTiet(maTaiKhoan, fromDate, toDate)
      .then(res => {
        if (res.errorCode === 200) setDetail(res.data);
        else toast.error(res.message ?? 'Không thể tải chi tiết tài khoản');
      })
      .catch(() => toast.error('Không thể kết nối đến máy chủ'))
      .finally(() => setLoading(false));
  }, [maTaiKhoan, fromDate, toDate]);

  const exportExcel = () => {
    if (!detail) return;
    const ws = XLSX.utils.json_to_sheet(detail.butToans.map(b => ({
      'Mã CT': b.maChungTu ?? '',
      'Ngày hạch toán': fmtDate(b.ngayHachToan),
      'Diễn giải': b.dienGiai ?? '',
      'Phát sinh Nợ': b.phatSinhNo,
      'Phát sinh Có': b.phatSinhCo,
      'Số dư lũy kế': b.soDuLuyKe,
    })));
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, `TK ${maTaiKhoan}`);
    XLSX.writeFile(wb, `so-cai-${maTaiKhoan}-${fromDate}-${toDate}.xlsx`);
  };

  return (
    <div className="space-y-4">
      {/* Back + header */}
      <div className="flex items-center justify-between">
        <button
          onClick={onBack}
          className="flex items-center gap-2 text-sm text-blue-600 hover:text-blue-800 transition-colors"
        >
          <ArrowLeft className="w-4 h-4" />
          Quay lại danh sách
        </button>
        <button
          onClick={exportExcel}
          disabled={!detail}
          className="flex items-center gap-1.5 px-3 py-2 text-sm bg-green-600 hover:bg-green-700 text-white rounded-lg transition-colors disabled:opacity-50"
        >
          <Download className="w-4 h-4" />
          Xuất Excel
        </button>
      </div>

      {loading ? (
        <div className="flex justify-center items-center py-16 text-gray-500">
          <RefreshCw className="w-5 h-5 animate-spin mr-2" />
          Đang tải...
        </div>
      ) : !detail ? (
        <div className="text-center py-16 text-gray-400">Không tìm thấy dữ liệu</div>
      ) : (
        <>
          {/* Summary card */}
          <div className="bg-white rounded-xl border border-gray-200 shadow-sm p-5">
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
              <div>
                <p className="text-xs text-gray-500 uppercase tracking-wide">Tài khoản</p>
                <h2 className="text-xl font-bold text-blue-800 font-mono">{detail.maTaiKhoan}</h2>
                <p className="text-gray-600 mt-0.5">{detail.tenTaiKhoan}</p>
                {detail.loaiTaiKhoan && (
                  <span className="mt-1 inline-block px-2 py-0.5 rounded-full text-xs bg-blue-100 text-blue-700">
                    {detail.loaiTaiKhoan}
                  </span>
                )}
              </div>
              <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 text-center">
                {[
                  { label: 'Số dư đầu kỳ', value: detail.soDuDauKy, color: 'text-gray-700' },
                  { label: 'Phát sinh Nợ', value: detail.phatSinhNo, color: 'text-blue-700' },
                  { label: 'Phát sinh Có', value: detail.phatSinhCo, color: 'text-red-600' },
                  { label: 'Số dư cuối kỳ', value: detail.soDuCuoiKy, color: detail.soDuCuoiKy >= 0 ? 'text-green-700' : 'text-red-600' },
                ].map(({ label, value, color }) => (
                  <div key={label} className="bg-gray-50 rounded-lg p-3">
                    <p className="text-xs text-gray-500">{label}</p>
                    <p className={`text-sm font-bold mt-1 ${color}`}>{fmt(value)}</p>
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Detail table */}
          <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
            {detail.butToans.length === 0 ? (
              <div className="text-center py-12 text-gray-400">Không có bút toán trong kỳ</div>
            ) : (
              <table className="w-full text-sm">
                <thead className="bg-gray-50 border-b border-gray-200">
                  <tr>
                    <th className="text-left px-4 py-3 font-semibold text-gray-700">Mã chứng từ</th>
                    <th className="text-left px-4 py-3 font-semibold text-gray-700">Ngày hạch toán</th>
                    <th className="text-left px-4 py-3 font-semibold text-gray-700">Diễn giải</th>
                    <th className="text-right px-4 py-3 font-semibold text-gray-700">Phát sinh Nợ</th>
                    <th className="text-right px-4 py-3 font-semibold text-gray-700">Phát sinh Có</th>
                    <th className="text-right px-4 py-3 font-semibold text-gray-700">Số dư lũy kế</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {/* Dòng số dư đầu kỳ */}
                  <tr className="bg-blue-50/60 font-medium">
                    <td className="px-4 py-2.5 text-blue-600" colSpan={2}>Số dư đầu kỳ</td>
                    <td className="px-4 py-2.5 text-blue-600 text-xs italic">
                      Kỳ từ {fmtDate(fromDate)} đến {fmtDate(toDate)}
                    </td>
                    <td className="px-4 py-2.5"></td>
                    <td className="px-4 py-2.5"></td>
                    <td className="px-4 py-2.5 text-right font-bold text-blue-700">{fmt(detail.soDuDauKy)}</td>
                  </tr>
                  {detail.butToans.map((b, i) => (
                    <tr key={i} className="hover:bg-gray-50 transition-colors">
                      <td className="px-4 py-2.5 font-mono text-blue-600 text-xs">{b.maChungTu ?? '—'}</td>
                      <td className="px-4 py-2.5 text-gray-600">{fmtDate(b.ngayHachToan)}</td>
                      <td className="px-4 py-2.5 text-gray-700">{b.dienGiai ?? '—'}</td>
                      <td className="px-4 py-2.5 text-right text-gray-700">
                        {b.phatSinhNo > 0 ? fmt(b.phatSinhNo) : ''}
                      </td>
                      <td className="px-4 py-2.5 text-right text-gray-700">
                        {b.phatSinhCo > 0 ? fmt(b.phatSinhCo) : ''}
                      </td>
                      <td className="px-4 py-2.5 text-right">
                        <BalanceCell value={b.soDuLuyKe} />
                      </td>
                    </tr>
                  ))}
                </tbody>
                <tfoot className="bg-gray-50 border-t-2 border-gray-200 font-semibold">
                  <tr>
                    <td colSpan={3} className="px-4 py-3 text-gray-700">
                      Cộng phát sinh ({detail.butToans.length} bút toán)
                    </td>
                    <td className="px-4 py-3 text-right text-blue-700">{fmt(detail.phatSinhNo)}</td>
                    <td className="px-4 py-3 text-right text-red-600">{fmt(detail.phatSinhCo)}</td>
                    <td className="px-4 py-3 text-right">
                      <BalanceCell value={detail.soDuCuoiKy} />
                    </td>
                  </tr>
                </tfoot>
              </table>
            )}
          </div>
        </>
      )}
    </div>
  );
}

// ============================
// MAIN COMPONENT
// ============================
export function GeneralLedger() {
  const currentYear = new Date().getFullYear();
  const [fromDate, setFromDate] = useState(`${currentYear}-01-01`);
  const [toDate, setToDate] = useState(`${currentYear}-12-31`);
  const [selectedAccount, setSelectedAccount] = useState<string | null>(null);

  const [appliedFrom, setAppliedFrom] = useState(fromDate);
  const [appliedTo, setAppliedTo] = useState(toDate);

  const applyFilter = () => {
    setAppliedFrom(fromDate);
    setAppliedTo(toDate);
    setSelectedAccount(null);
  };

  return (
    <div className="space-y-6">
      {/* Page header */}
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
            <BookOpen className="w-6 h-6 text-blue-600" />
            Sổ Cái
          </h1>
          <p className="text-gray-500 text-sm mt-0.5">
            Tổng hợp và chi tiết số dư theo từng tài khoản kế toán
          </p>
        </div>

        {/* Period filter */}
        <div className="flex flex-wrap items-center gap-2 bg-white rounded-xl border border-gray-200 shadow-sm px-4 py-3">
          <div className="flex items-center gap-2">
            <label className="text-xs text-gray-500 whitespace-nowrap">Từ ngày</label>
            <input
              type="date"
              value={fromDate}
              onChange={e => setFromDate(e.target.value)}
              className="border border-gray-300 rounded-lg px-2 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="flex items-center gap-2">
            <label className="text-xs text-gray-500 whitespace-nowrap">Đến ngày</label>
            <input
              type="date"
              value={toDate}
              onChange={e => setToDate(e.target.value)}
              className="border border-gray-300 rounded-lg px-2 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <button
            onClick={applyFilter}
            className="flex items-center gap-1.5 px-3 py-1.5 text-sm bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
          >
            <Search className="w-4 h-4" />
            Xem
          </button>
        </div>
      </div>

      {/* Content */}
      {selectedAccount ? (
        <AccountDetailView
          maTaiKhoan={selectedAccount}
          fromDate={appliedFrom}
          toDate={appliedTo}
          onBack={() => setSelectedAccount(null)}
        />
      ) : (
        <AccountListView
          fromDate={appliedFrom}
          toDate={appliedTo}
          onSelectAccount={setSelectedAccount}
        />
      )}
    </div>
  );
}
