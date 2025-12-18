import RegistrationDocuments from './pages/RegistrationDocuments.vue';
import FolderManager from './pages/FolderManager.vue';

const routes = [
    // {path: '/', name:'Home', component: Home},
    {path: '/', name: 'registration', component: RegistrationDocuments},
    {path: '/folderManager/:id', name: 'folderManager', component: FolderManager, props:true,},
]

export default routes