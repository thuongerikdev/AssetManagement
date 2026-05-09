import { useState, useRef, useEffect, useCallback } from 'react';
import { ChevronDown, Crown } from 'lucide-react';

interface UserRole {
  roleID: number;
  roleName: string;
}

export interface UserOption {
  userID: number;
  userName: string;
  email?: string;
  profile?: { firstName?: string; lastName?: string } | null;
  roles?: UserRole[];
}

interface Props {
  users: UserOption[];
  value: number | undefined;
  onChange: (userId: number | undefined) => void;
  disabled?: boolean;
  isLoading?: boolean;
  noDeptSelected?: boolean;
}

function isTruongPhong(user: UserOption): boolean {
  return user.roles?.some(r => r.roleName.startsWith('truong_phong')) ?? false;
}

function getTruongPhongLabel(user: UserOption): string {
  const role = user.roles?.find(r => r.roleName.startsWith('truong_phong'));
  if (!role) return 'Trưởng phòng';
  if (role.roleName === 'truong_phong_ke_toan') return 'Trưởng P. Kế toán';
  if (role.roleName === 'truong_phong_ky_thuat') return 'Trưởng P. Kỹ thuật';
  return 'Trưởng phòng';
}

function getFullName(user: UserOption): string {
  const p = user.profile;
  if (p?.lastName || p?.firstName) {
    return `${p.lastName ?? ''} ${p.firstName ?? ''}`.trim();
  }
  return user.userName;
}

export function UserSelectDropdown({ users, value, onChange, disabled, isLoading, noDeptSelected }: Props) {
  const [open, setOpen] = useState(false);
  const [dropdownStyle, setDropdownStyle] = useState<React.CSSProperties>({});
  const ref = useRef<HTMLDivElement>(null);
  const triggerRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    const handle = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener('mousedown', handle);
    return () => document.removeEventListener('mousedown', handle);
  }, []);

  // Cập nhật vị trí dropdown mỗi khi mở để tránh bị khuất trong modal overflow
  const updateDropdownPosition = useCallback(() => {
    if (!triggerRef.current) return;
    const rect = triggerRef.current.getBoundingClientRect();
    const spaceBelow = window.innerHeight - rect.bottom;
    const dropdownHeight = Math.min(256, users.length * 48 + 60);

    if (spaceBelow < dropdownHeight && rect.top > dropdownHeight) {
      // Mở lên trên nếu không đủ chỗ bên dưới
      setDropdownStyle({
        position: 'fixed',
        bottom: window.innerHeight - rect.top,
        left: rect.left,
        width: rect.width,
        zIndex: 9999,
      });
    } else {
      setDropdownStyle({
        position: 'fixed',
        top: rect.bottom,
        left: rect.left,
        width: rect.width,
        zIndex: 9999,
      });
    }
  }, [users.length]);

  const handleToggle = () => {
    if (!open) updateDropdownPosition();
    setOpen(o => !o);
  };

  // Sort: trưởng phòng lên đầu
  const heads = users.filter(isTruongPhong);
  const staff = users.filter(u => !isTruongPhong(u));

  const selectedUser = users.find(u => u.userID === value);
  const isDisabled = disabled || isLoading || users.length === 0;

  const placeholderText = isLoading
    ? 'Đang tải...'
    : noDeptSelected
    ? '-- Chọn phòng ban trước --'
    : users.length === 0
    ? '-- P. Ban này chưa có NV --'
    : '-- Chọn nhân viên --';

  return (
    <div ref={ref} className="relative">
      {/* Trigger button */}
      <button
        ref={triggerRef}
        type="button"
        disabled={isDisabled}
        onClick={handleToggle}
        className="w-full px-4 py-2.5 border border-gray-300 rounded-lg text-left flex items-center justify-between bg-white disabled:bg-gray-50 disabled:text-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors"
      >
        <span className="flex items-center gap-1.5 truncate min-w-0">
          {selectedUser ? (
            <>
              {isTruongPhong(selectedUser) && (
                <Crown className="w-3.5 h-3.5 text-amber-500 flex-shrink-0" />
              )}
              <span className="truncate text-sm text-gray-900">
                {getFullName(selectedUser)}
                <span className="text-gray-500"> ({selectedUser.userName})</span>
              </span>
            </>
          ) : (
            <span className="text-gray-400 text-sm">{placeholderText}</span>
          )}
        </span>
        <ChevronDown
          className={`w-4 h-4 text-gray-400 flex-shrink-0 ml-2 transition-transform ${open ? 'rotate-180' : ''}`}
        />
      </button>

      {/* Dropdown list — dùng fixed để không bị khuất bởi overflow của modal */}
      {open && !isDisabled && (
        <div style={dropdownStyle} className="bg-white border border-gray-200 rounded-lg shadow-xl max-h-64 overflow-y-auto">
          {/* Clear option */}
          <div
            className="px-4 py-2 text-sm text-gray-400 hover:bg-gray-50 cursor-pointer border-b border-gray-100"
            onMouseDown={() => { onChange(undefined); setOpen(false); }}
          >
            -- Chọn nhân viên --
          </div>

          {/* Trưởng phòng section */}
          {heads.length > 0 && (
            <>
              <div className="px-3 py-1 text-xs font-semibold text-amber-700 bg-amber-50 flex items-center gap-1.5 sticky top-0">
                <Crown className="w-3 h-3" />
                Trưởng phòng
              </div>
              {heads.map(user => (
                <div
                  key={user.userID}
                  onMouseDown={() => { onChange(user.userID); setOpen(false); }}
                  className={`px-4 py-2.5 cursor-pointer flex items-center gap-2.5 hover:bg-amber-50 transition-colors ${value === user.userID ? 'bg-amber-50' : ''}`}
                >
                  <div className="w-7 h-7 rounded-full bg-amber-100 flex items-center justify-center flex-shrink-0">
                    <Crown className="w-3.5 h-3.5 text-amber-600" />
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="text-sm font-medium text-gray-900 truncate">{getFullName(user)}</div>
                    <div className="text-xs text-amber-600 truncate">{user.userName} · {getTruongPhongLabel(user)}</div>
                  </div>
                  {value === user.userID && (
                    <span className="text-amber-500 text-xs">✓</span>
                  )}
                </div>
              ))}
            </>
          )}

          {/* Nhân viên section */}
          {staff.length > 0 && (
            <>
              {heads.length > 0 && <div className="border-t border-gray-100" />}
              <div className="px-3 py-1 text-xs font-semibold text-gray-500 bg-gray-50 sticky top-0">
                Nhân viên
              </div>
              {staff.map(user => (
                <div
                  key={user.userID}
                  onMouseDown={() => { onChange(user.userID); setOpen(false); }}
                  className={`px-4 py-2.5 cursor-pointer flex items-center gap-2.5 hover:bg-gray-50 transition-colors ${value === user.userID ? 'bg-blue-50' : ''}`}
                >
                  <div className="w-7 h-7 rounded-full bg-gray-100 flex items-center justify-center flex-shrink-0 text-xs font-medium text-gray-600">
                    {getFullName(user).charAt(0).toUpperCase()}
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="text-sm text-gray-900 truncate">{getFullName(user)}</div>
                    <div className="text-xs text-gray-400 truncate">{user.userName}</div>
                  </div>
                  {value === user.userID && (
                    <span className="text-blue-500 text-xs">✓</span>
                  )}
                </div>
              ))}
            </>
          )}
        </div>
      )}
    </div>
  );
}
