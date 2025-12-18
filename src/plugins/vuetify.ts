import { createVuetify } from 'vuetify'
import 'vuetify/styles'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
    components,
    directives,
    icons: { defaultSet: 'mdi', aliases, sets: { mdi }},
    theme: { defaultTheme: 'companyTheme', themes: {
        companyTheme: { dark: false, colors: {
            primary:   '#87BAC3',
            secondary: '#16476A',
            background:'#F4F6FB',
            Success:'#41A67E',
            alarm:   '#842A3B',
            text:      '#000000',}
        }
    }
    }
})

export default vuetify