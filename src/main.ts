import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import { createRouter, createWebHistory } from 'vue-router'
import routes from './routes'
import vuetify from './plugins/vuetify.ts'

const app = createApp(App)

const router = createRouter({
    history: createWebHistory(),
    routes,
})

app.use(createPinia())
app.use(router)
app.use(vuetify)
app.mount('#app')