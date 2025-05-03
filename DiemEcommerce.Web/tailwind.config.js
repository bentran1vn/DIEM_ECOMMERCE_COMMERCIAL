/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml",
    "./Pages/**/*.cs",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#1e40af',
        'primary-dark': '#1e3a8a',
        'secondary': '#6b7280',
        'accent': '#f97316',
        'success': '#10b981',
        'warning': '#f59e0b',
        'danger': '#ef4444',
        'light': '#f3f4f6',
        'dark': '#1f2937'
      },
      fontFamily: {
        'sans': ['Inter', 'ui-sans-serif', 'system-ui'],
        'serif': ['ui-serif', 'Georgia'],
        'mono': ['ui-monospace', 'SFMono-Regular'],
      }
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}