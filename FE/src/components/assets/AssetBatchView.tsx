import { useState, useEffect } from 'react';
import { Link } from 'react-router';
import { ArrowLeft, Layers, Eye, Send, ArrowLeftRight, Wrench, Trash2, Download, Database } from 'lucide-react';
import { seedSampleBatchData } from '../../utils/seedData';
import { toast, Toaster } from "sonner";



interface Asset {
  id: string;
  code: string;
  name: string;
  type: string;
  department: string;
  user: string;
  originalValue: number;
  remainingValue: number;
  status: 'active' | 'maintenance' | 'liquidated';
  batchId?: string | null;
  batchIndex?: number;
  batchTotal?: number;
  serialNumber?: string;
  createdAt?: string;
}

interface BatchGroup {
  batchId: string;
  assets: Asset[];
  totalValue: number;
  createdAt: string;
}

const statusConfig = {
  active: { label: 'Đang sử dụng', color: 'bg-green-100 text-green-700' },
  maintenance: { label: 'Bảo trì', color: 'bg-orange-100 text-orange-700' },
  liquidated: { label: 'Đã thanh lý', color: 'bg-gray-100 text-gray-700' },
};

export function AssetBatchView() {
  const [batches, setBatches] = useState<BatchGroup[]>([]);
  const [expandedBatch, setExpandedBatch] = useState<string | null>(null);

  const loadBatches = () => {
    // Load assets from localStorage
    const storedAssets = JSON.parse(localStorage.getItem('assets') || '[]');
    
    // Group by batchId
    const batchMap = new Map<string, Asset[]>();
    
    storedAssets.forEach((asset: any, index: number) => {
      if (asset.batchId) {
        const assetWithId = {
          ...asset,
          id: `stored-${index}`,
          type: asset.assetGroup || 'N/A',
          user: asset.recipient || 'Chưa cấp phát',
          remainingValue: parseFloat(asset.originalValue) || 0,
        };
        
        if (!batchMap.has(asset.batchId)) {
          batchMap.set(asset.batchId, []);
        }
        batchMap.get(asset.batchId)!.push(assetWithId);
      }
    });

    // Convert to BatchGroup array
    const batchGroups: BatchGroup[] = Array.from(batchMap.entries()).map(([batchId, assets]) => ({
      batchId,
      assets: assets.sort((a, b) => (a.batchIndex || 0) - (b.batchIndex || 0)),
      totalValue: assets.reduce((sum, asset) => sum + (parseFloat(asset.originalValue as any) || 0), 0),
      createdAt: assets[0]?.createdAt || new Date().toISOString(),
    }));

    setBatches(batchGroups);
  };

  useEffect(() => {
    loadBatches();
  }, []);

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('vi-VN', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <div className="flex items-center gap-3">
            <Link
              to="/assets"
              className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
            >
              <ArrowLeft className="w-5 h-5 text-gray-600" />
            </Link>
            <div>
              <h1 className="font-bold text-gray-900">Quản lý Tài sản theo Lô</h1>
              <p className="text-sm text-gray-500 mt-1">
                Xem và quản lý các tài sản được tạo theo batch
              </p>
            </div>
          </div>
        </div>
        <div className="flex gap-3">
          <Link
            to="/assets"
            className="flex items-center gap-2 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Xem tất cả tài sản
          </Link>
          <Link
            to="/assets/new?mode=batch"
            className="flex items-center gap-2 px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition-colors"
          >
            <Layers className="w-5 h-5" />
            Tạo Lô mới
          </Link>
        </div>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <div className="flex items-center gap-3">
            <div className="bg-purple-100 rounded-lg p-3">
              <Layers className="w-6 h-6 text-purple-600" />
            </div>
            <div>
              <p className="text-sm text-gray-500">Tổng số lô</p>
              <p className="text-2xl font-bold text-gray-900">{batches.length}</p>
            </div>
          </div>
        </div>
        
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <div className="flex items-center gap-3">
            <div className="bg-blue-100 rounded-lg p-3">
              <Layers className="w-6 h-6 text-blue-600" />
            </div>
            <div>
              <p className="text-sm text-gray-500">Tổng tài sản</p>
              <p className="text-2xl font-bold text-gray-900">
                {batches.reduce((sum, batch) => sum + batch.assets.length, 0)}
              </p>
            </div>
          </div>
        </div>
        
        <div className="bg-white rounded-lg border border-gray-200 p-6">
          <div className="flex items-center gap-3">
            <div className="bg-green-100 rounded-lg p-3">
              <Download className="w-6 h-6 text-green-600" />
            </div>
            <div>
              <p className="text-sm text-gray-500">Tổng giá trị</p>
              <p className="text-xl font-bold text-gray-900">
                {formatCurrency(batches.reduce((sum, batch) => sum + batch.totalValue, 0))}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Batch List */}
      {batches.length === 0 ? (
        <div className="bg-white rounded-lg border border-gray-200 p-12 text-center">
          <Layers className="w-16 h-16 text-gray-300 mx-auto mb-4" />
          <h3 className="font-semibold text-gray-900 mb-2">Chưa có tài sản theo lô</h3>
          <p className="text-sm text-gray-500 mb-4">
            Tạo tài sản theo lô để quản lý nhiều tài sản cùng loại một cách dễ dàng
          </p>
          <div className="flex gap-3 justify-center">
            <button
              onClick={() => {
                seedSampleBatchData();
                loadBatches();
                toast.success('Đã tải dữ liệu mẫu!', {
                  description: 'Tạo 2 lô mẫu: 5 ghế và 10 bàn làm việc',
                });
              }}
              className="inline-flex items-center gap-2 px-4 py-2 border border-gray-300 bg-white text-gray-700 rounded-lg hover:bg-gray-50 transition-colors"
            >
              <Database className="w-5 h-5" />
              Tải dữ liệu mẫu
            </button>
            <Link
              to="/assets/new?mode=batch"
              className="inline-flex items-center gap-2 px-4 py-2 bg-purple-600 text-white rounded-lg hover:bg-purple-700 transition-colors"
            >
              <Layers className="w-5 h-5" />
              Tạo Lô đầu tiên
            </Link>
          </div>
        </div>
      ) : (
        <div className="space-y-4">
          {batches.map((batch) => (
            <div key={batch.batchId} className="bg-white rounded-lg border border-gray-200 overflow-hidden">
              {/* Batch Header */}
              <div
                className="bg-gradient-to-r from-purple-50 to-pink-50 border-b border-purple-200 p-4 cursor-pointer hover:from-purple-100 hover:to-pink-100 transition-colors"
                onClick={() => setExpandedBatch(expandedBatch === batch.batchId ? null : batch.batchId)}
              >
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-4">
                    <div className="bg-purple-600 rounded-lg p-3">
                      <Layers className="w-6 h-6 text-white" />
                    </div>
                    <div>
                      <div className="flex items-center gap-3">
                        <h3 className="font-semibold text-purple-900">
                          {batch.assets[0]?.name || 'Tài sản'}
                        </h3>
                        <span className="px-3 py-1 bg-purple-600 text-white rounded-full text-xs font-medium">
                          {batch.assets.length} tài sản
                        </span>
                      </div>
                      <div className="flex items-center gap-4 mt-2 text-sm text-purple-700">
                        <span>Mã lô: {batch.batchId}</span>
                        <span>•</span>
                        <span>Tạo ngày: {formatDate(batch.createdAt)}</span>
                        <span>•</span>
                        <span className="font-medium">
                          Từ {batch.assets[0]?.code} đến {batch.assets[batch.assets.length - 1]?.code}
                        </span>
                      </div>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-sm text-purple-700">Tổng giá trị</p>
                    <p className="text-xl font-bold text-purple-900">{formatCurrency(batch.totalValue)}</p>
                  </div>
                </div>
              </div>

              {/* Expanded Batch Details */}
              {expandedBatch === batch.batchId && (
                <div className="overflow-x-auto">
                  <table className="w-full">
                    <thead className="bg-gray-50 border-b border-gray-200">
                      <tr>
                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-700 uppercase">
                          STT
                        </th>
                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-700 uppercase">
                          Mã TS
                        </th>
                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-700 uppercase">
                          Serial Number
                        </th>
                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-700 uppercase">
                          Phòng ban
                        </th>
                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-700 uppercase">
                          Người dùng
                        </th>
                        <th className="px-4 py-3 text-right text-xs font-medium text-gray-700 uppercase">
                          Nguyên giá
                        </th>
                        <th className="px-4 py-3 text-center text-xs font-medium text-gray-700 uppercase">
                          Trạng thái
                        </th>
                        <th className="px-4 py-3 text-center text-xs font-medium text-gray-700 uppercase">
                          Thao tác
                        </th>
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-200">
                      {batch.assets.map((asset) => (
                        <tr key={asset.id} className="hover:bg-gray-50 transition-colors">
                          <td className="px-4 py-3 whitespace-nowrap">
                            <span className="text-sm font-medium text-gray-900">{asset.batchIndex}</span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap">
                            <span className="text-sm font-mono text-blue-600">{asset.code}</span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap">
                            <span className="text-sm text-gray-600">
                              {asset.serialNumber || (
                                <span className="text-gray-400 italic">Chưa cập nhật</span>
                              )}
                            </span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap">
                            <span className="text-sm text-gray-600">{asset.department}</span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap">
                            <span className="text-sm text-gray-600">{asset.user}</span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap text-right">
                            <span className="text-sm text-gray-900">{formatCurrency(asset.originalValue)}</span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap text-center">
                            <span className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${statusConfig[asset.status].color}`}>
                              {statusConfig[asset.status].label}
                            </span>
                          </td>
                          <td className="px-4 py-3 whitespace-nowrap text-center">
                            <div className="flex items-center justify-center gap-2">
                              <Link
                                to={`/assets/${asset.id}`}
                                className="p-1 text-blue-600 hover:bg-blue-50 rounded transition-colors"
                                title="Chi tiết"
                              >
                                <Eye className="w-4 h-4" />
                              </Link>
                              <button
                                className="p-1 text-green-600 hover:bg-green-50 rounded transition-colors"
                                title="Cấp phát"
                              >
                                <Send className="w-4 h-4" />
                              </button>
                              <button
                                className="p-1 text-orange-600 hover:bg-orange-50 rounded transition-colors"
                                title="Điều chuyển"
                              >
                                <ArrowLeftRight className="w-4 h-4" />
                              </button>
                              <button
                                className="p-1 text-purple-600 hover:bg-purple-50 rounded transition-colors"
                                title="Bảo trì"
                              >
                                <Wrench className="w-4 h-4" />
                              </button>
                              <button
                                className="p-1 text-red-600 hover:bg-red-50 rounded transition-colors"
                                title="Thanh lý"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </div>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
