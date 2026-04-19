import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { toast } from 'sonner';
import { assetApi, TaiSan } from '../api/assetApi';
import { departmentApi, Department } from '../api/departmentApi';
import { assetCategoryApi, AssetCategory } from '../api/assetCategoryApi';

interface GlobalContextType {
  assets: TaiSan[];
  departments: Department[];
  categories: AssetCategory[];
  isLoadingGlobal: boolean;
  refreshData: () => Promise<void>;
}

const GlobalContext = createContext<GlobalContextType | undefined>(undefined);

export function GlobalProvider({ children }: { children: ReactNode }) {
  const [assets, setAssets] = useState<TaiSan[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [categories, setCategories] = useState<AssetCategory[]>([]);
  const [isLoadingGlobal, setIsLoadingGlobal] = useState(true);

  // Hàm này gọi API lấy tất cả dữ liệu (Chỉ chạy 1 lần khi mở web)
  const fetchData = async () => {
    setIsLoadingGlobal(true);
    try {
      const [assetRes, deptRes, catRes] = await Promise.all([
        assetApi.getAll(),
        departmentApi.getAll(),
        assetCategoryApi.getAll()
      ]);

      if (assetRes.errorCode === 200) setAssets(assetRes.data);
      if (deptRes.errorCode === 200) setDepartments(deptRes.data);
      if (catRes.errorCode === 200) setCategories(catRes.data);
    } catch (error) {
      toast.error('Lỗi khi tải dữ liệu hệ thống.');
    } finally {
      setIsLoadingGlobal(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <GlobalContext.Provider value={{ assets, departments, categories, isLoadingGlobal, refreshData: fetchData }}>
      {children}
    </GlobalContext.Provider>
  );
}

// Hàm để các file khác lấy dữ liệu ra dùng
export const useGlobalData = () => {
  const context = useContext(GlobalContext);
  if (!context) throw new Error('useGlobalData phải nằm trong GlobalProvider');
  return context;
};