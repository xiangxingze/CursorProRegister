// Mock turnstile object
window.turnstile = {
    ready: true,
    implicitRender: true,
    render: function(container, options) {
        console.log('Turnstile render called');
        
        // Generate a mock token that matches the format
        const mockToken = {
            redirect_uri: 'https://cursor.sh/api/auth/callback',
            bot_detection_token: '0.FDTIovlwhV-ydipOzErzJ3ivUfeMtD4hKkquHlt3_c-WX2tIJA1L3ThE0VvK5g6MHszXe7T57lAy_fQrvcnaidiDPrlbFQ1u87zBePLzllqhJ60JaXd6pa-UlfRVv5FK8GTU22CZEHnmfSNlEgIP2Gy5Zl1g6cnHKmj9ljTtEjDhz5fWh9Q7N_z6C6OowmPRmlb729mw_i63SNqATexUueEt8QAHiTmDUapcr_OICdvucSg6l0fsEhhTcqsG5glz9RjCrAgiTz8GRXwxe6JTjQLUXxiPEW9VsneSYSSnYV-ZLfiwVDvShzIteYExN0JApEwjVfA_wE57RTJMkjP9CA5L6EPup992AD7LGXwiQjEbA24daInVKjx4vulB_5vhGa3QWM2LUBkomqF50NAebTYDxeTVxmN05mdHfjFLcvobGq8nCgOmhDjzNXR4V5k4Ur-Rrn5bt1O3QXmFqP0tcSLQbNBN9CPlcxdgD-TBW4Ixiwv_fR9pN6nQuJbqzfVUD-yfAmYcdqbpl_LqlJg5TetKXmOugoSChgISsU7UFVSc4PTrUJOK47yC2D0bZYmxph7OBZ0x2dKca1XoUQfIVkxNNxy64APk_vAmj0GIPlLMyylNTILyFPz5Ypa8r3J8t28CtUbHwMdZT4ErpPHjiXvy-pbMK9qwa4IM6Rlp19R8DMRn2nJM9OzcokGfiFRK2jkqvuGrhmXMg3PcUCSJ1QWNdyB-LspoKN0xa4Qiwp-64-nVqO0WTARTJ8IJwT1R5ausotfxwfUQlgFaA3o-Rw3ZT7-Kg00MU973H90D0yML2woLed35i068SveDJAMjYU8u7avFQPEYS288ioT-7xuP7VCyq6UvnEIGbCc_Un0.DDPKYmx1SoSaZOc9CTiLyw.61bc26684bcb187eaeaee3ba334edd97c00087ddeb9b5a0d4c122d0adbad41f2',
            browser_supports_passkeys: true
        };

        // 延迟执行回调
        setTimeout(() => {
            if (typeof options.callback === 'function') {
                console.log('Calling callback with token');
                options.callback(mockToken.bot_detection_token);
                
                // 触发自定义事件
                const event = new CustomEvent('turnstile-callback', { detail: mockToken });
                window.dispatchEvent(event);
                document.dispatchEvent(event);
            }
        }, 1000);

        return 'mocked_widget_id';
    }
};

// 监听表单提交
const handleFormSubmit = () => {
    const form = document.querySelector('form');
    if (form) {
        const formData = new FormData(form);
        console.log('Form data:', Object.fromEntries(formData));
    }
};

// 设置表单监听器
document.addEventListener('DOMContentLoaded', () => {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', handleFormSubmit);
    });
});

// 设置随机鼠标坐标
function setRandomMouseCoordinates() {
    const screenX = Math.floor(Math.random() * (1200 - 800 + 1)) + 800;
    const screenY = Math.floor(Math.random() * (600 - 400 + 1)) + 400;

    try {
        Object.defineProperty(MouseEvent.prototype, 'screenX', { value: screenX });
        Object.defineProperty(MouseEvent.prototype, 'screenY', { value: screenY });
    } catch (e) {
        console.error('设置鼠标事件属性失败:', e);
    }
}

// 初始化 Turnstile 补丁
function initTurnstilePatch() {
    try {
        // 保存原始的回调
        window.onloadTurnstileCallback__cf_original = window.onloadTurnstileCallback__cf_turnstile;

        // 创建新的回调处理函数
        window.onloadTurnstileCallback__cf_turnstile = function() {
            console.log('Turnstile callback loaded');
            try {
                if (window.onloadTurnstileCallback__cf_original) {
                    window.onloadTurnstileCallback__cf_original();
                }
            } catch (e) {
                console.error('执行原始回调失败:', e);
            }
            
            // 注入我们的 turnstile 对象
            window.turnstile = {
                ready: true,
                implicitRender: true,
                render: function(container, options) {
                    console.log('Turnstile render called', container, options);
                    try {
                        // 生成一个看起来真实的token
                        const token = '0.' + Math.random().toString(36).substring(2) + '.' + Date.now();
                        
                        // 创建一个假的验证框
                        const mockWidget = document.createElement('div');
                        mockWidget.style.width = '100%';
                        mockWidget.style.height = '100%';
                        mockWidget.style.backgroundColor = '#1a1a1a';
                        mockWidget.style.border = '1px solid #333';
                        mockWidget.style.borderRadius = '4px';
                        
                        // 清空容器并添加假的验证框
                        if (typeof container === 'string') {
                            container = document.getElementById(container);
                        }
                        if (container) {
                            container.innerHTML = '';
                            container.appendChild(mockWidget);
                        }

                        // 延迟调用回调
                        setTimeout(() => {
                            try {
                                console.log('Calling callback with token:', token);
                                if (typeof options.callback === 'function') {
                                    options.callback(token);
                                }
                                
                                // 触发成功事件
                                const event = new CustomEvent('turnstile-success', { 
                                    detail: { 
                                        token,
                                        widgetId: 'widget_' + Math.random().toString(36).substring(2)
                                    } 
                                });
                                window.dispatchEvent(event);
                                document.dispatchEvent(event);
                                
                                // 移除验证框
                                setTimeout(() => {
                                    if (container) {
                                        container.style.display = 'none';
                                    }
                                    // 移除验证提示文本
                                    const verifyText = document.querySelector('p.rt-Text.rt-r-lt-start');
                                    if (verifyText) {
                                        verifyText.style.display = 'none';
                                    }
                                }, 500);
                            } catch (e) {
                                console.error('处理回调时发生错误:', e);
                            }
                        }, 1000);

                        return 'widget_' + Math.random().toString(36).substring(2);
                    } catch (e) {
                        console.error('渲染验证框时发生错误:', e);
                        return null;
                    }
                },
                getResponse: function() {
                    try {
                        return '0.' + Math.random().toString(36).substring(2) + '.' + Date.now();
                    } catch (e) {
                        console.error('获取响应时发生错误:', e);
                        return null;
                    }
                },
                reset: function() {
                    console.log('Turnstile reset called');
                }
            };

            // 处理已存在的验证框
            handleExistingChallenges();
        };

        // 处理已存在的验证框
        function handleExistingChallenges() {
            try {
                const challenges = document.querySelectorAll('[id^=cf-turnstile]');
                challenges.forEach(challenge => {
                    try {
                        const container = challenge.id;
                        const options = {
                            callback: function(token) {
                                console.log('Challenge completed with token:', token);
                            }
                        };
                        window.turnstile.render(container, options);
                    } catch (e) {
                        console.error('处理验证框失败:', e);
                    }
                });
            } catch (e) {
                console.error('查找验证框失败:', e);
            }
        }

        // 定期检查新的验证框
        setInterval(handleExistingChallenges, 1000);
    } catch (e) {
        console.error('初始化 Turnstile 补丁失败:', e);
    }
}

// 设置随机鼠标坐标
setRandomMouseCoordinates();

// 在页面加载时初始化补丁
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initTurnstilePatch);
} else {
    initTurnstilePatch();
} 