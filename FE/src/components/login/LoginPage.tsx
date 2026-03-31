import React, { useEffect, useState } from 'react';
import { useNavigate, Link } from 'react-router';
import { authApi } from '../../api/authApi';
import { toast } from 'sonner';
import './style.css'; 

export function LoginPage() {
  const navigate = useNavigate();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  // Kiểm tra nếu đã có token thì đá thẳng vào trong luôn, khỏi bắt đăng nhập lại
  useEffect(() => {
    const token = localStorage.getItem('access_token');
    if (token) {
      navigate('/', { replace: true });
    }
  }, [navigate]);

  // Animation mắt - miệng 
  useEffect(() => {
    const passwordField = document.getElementById('password') as HTMLInputElement | null;

    const handleMouseMove = (event: MouseEvent) => {
      if (!document.querySelector('#password:is(:focus)') && !document.querySelector('#password:is(:user-invalid)')) {
        const eyes = document.getElementsByClassName('eye') as HTMLCollectionOf<HTMLElement>;
        for (let i = 0; i < eyes.length; i++) {
          const eye = eyes[i];
          const rect = eye.getBoundingClientRect();
          const x = rect.left + 10;
          const y = rect.top + 10;
          const rad = Math.atan2(event.pageX - x, event.pageY - y);
          const rot = rad * (180 / Math.PI) * -1 + 180;
          eye.style.transform = `rotate(${rot}deg)`;
        }
      }
    };

    const handleFocusPassword = () => {
      const face = document.getElementById('face');
      if (face) face.style.transform = 'translateX(30px)';
      const eyes = document.getElementsByClassName('eye') as HTMLCollectionOf<HTMLElement>;
      for (let i = 0; i < eyes.length; i++) eyes[i].style.transform = `rotate(100deg)`;
    };

    const handleFocusOutPassword = (event: FocusEvent) => {
      const face = document.getElementById('face');
      const ball = document.getElementById('ball');
      if (face) face.style.transform = 'translateX(0)';
      const target = event.target as HTMLInputElement;
      if (ball) ball.classList.toggle('sad');
      if (!target.checkValidity()) {
        const eyes = document.getElementsByClassName('eye') as HTMLCollectionOf<HTMLElement>;
        for (let i = 0; i < eyes.length; i++) eyes[i].style.transform = `rotate(215deg)`;
      }
    };

    const handleSubmitHover = () => {
      const ball = document.getElementById('ball');
      if (ball) ball.classList.toggle('look_at');
    };

    document.addEventListener('mousemove', handleMouseMove);
    passwordField?.addEventListener('focus', handleFocusPassword);
    passwordField?.addEventListener('focusout', handleFocusOutPassword);
    const submitButton = document.getElementById('submit');
    submitButton?.addEventListener('mouseover', handleSubmitHover);
    submitButton?.addEventListener('mouseout', handleSubmitHover);

    return () => {
      document.removeEventListener('mousemove', handleMouseMove);
      passwordField?.removeEventListener('focus', handleFocusPassword);
      passwordField?.removeEventListener('focusout', handleFocusOutPassword);
      submitButton?.removeEventListener('mouseover', handleSubmitHover);
      submitButton?.removeEventListener('mouseout', handleSubmitHover);
    };
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const response = await authApi.login({ userName: username.trim(), password });
      
      // Lúc này TypeScript đã nhận diện được cấu trúc mới cực chuẩn
      if (response.errorCode === 200 && response.data?.token) {
        
        // 1. Lưu token để dùng cho các API khác
        localStorage.setItem('access_token', response.data.token);
        
        // 2. Lưu luôn thông tin user (ID, Tên, Email...) để hiển thị trên Header/Sidebar
        const userInfo = {
          userID: response.data.userID,
          userName: response.data.userName,
          email: response.data.email,
          permissions: response.data.permissions
        };
        localStorage.setItem('user_info', JSON.stringify(userInfo));
        
        toast.success('Đăng nhập thành công!');
        
        // 3. Chuyển hướng vào trang Dashboard
        navigate('/', { replace: true });
      } else {
        toast.error(response.errorMessage || 'Đăng nhập thất bại! Sai tài khoản hoặc mật khẩu.');
      }
    } catch (error: any) {
      toast.error('Lỗi kết nối đến máy chủ. Vui lòng thử lại!');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="login-main-wrapper">
      <section className="form login-section-wrapper">
        <div className="logo">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" d="M21 7.5l-2.25-1.313M21 7.5v2.25m0-2.25l-2.25 1.313M3 7.5l2.25-1.313M3 7.5l2.25 1.313M3 7.5v2.25m9 3l2.25-1.313M12 12.75l-2.25-1.313M12 12.75V15m0 6.75l2.25-1.313M12 21.75V19.5m0 2.25l-2.25-1.313m0-16.875L12 2.25l2.25 1.313M21 14.25v2.25l-2.25 1.313m-13.5 0L3 16.5v-2.25" />
          </svg>
        </div>

        <h1 className="form__title">Hệ thống Quản lý Tài sản</h1>
        <p className="form__description">Vui lòng đăng nhập để tiếp tục</p>

        <form onSubmit={handleSubmit}>
          <label className="form-control__label">Tên đăng nhập</label>
          <input
            type="text"
            className="form-control"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />

          <label className="form-control__label">Mật khẩu</label>
          <div className="password-field">
            <input
              type="password"
              className="form-control"
              minLength={4}
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88" />
            </svg>
          </div>

          <div className="password__settings">
            <label className="password__settings__remember">
              <input type="checkbox" />
              <span className="custom__checkbox">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="2" stroke="currentColor" className="w-6 h-6">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 12.75l6 6 9-13.5" />
                </svg>
              </span>
              Nhớ đăng nhập
            </label>
            <a href="#">Quên mật khẩu?</a>
          </div>

          <button type="submit" className="form__submit" id="submit" disabled={isLoading}>
            {isLoading ? 'Đang xác thực...' : 'Đăng Nhập'}
          </button>
        </form>
      </section>

      {/* Khu vực animation mắt miệng */}
      <section className="form__animation login-section-wrapper">
        <div id="ball">
          <div className="ball">
            <div id="face">
              <div className="ball__eyes">
                <div className="eye_wrap"><span className="eye"></span></div>
                <div className="eye_wrap"><span className="eye"></span></div>
              </div>
              <div className="ball__mouth"></div>
            </div>
          </div>
        </div>
        <div className="ball__shadow"></div>
      </section>
    </main>
  );
}