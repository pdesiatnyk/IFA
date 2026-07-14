import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

// The C# API (csharp/IfaUdi.Parser.Api) runs on :5081 in dev — see its Properties/launchSettings.json.
export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5081',
        changeOrigin: true,
      },
    },
  },
});
