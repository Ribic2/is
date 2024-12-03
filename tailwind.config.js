/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml'
  ],
  theme: {
    extend: {
      primary: '#A5A6A9',   
      secondary: '#DFE0E2', 
      neutral: '#FFFFFF',  
      accent: '#F45844',    
    },
  },
  plugins: [],
}

