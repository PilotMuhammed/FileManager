<template>
    <v-container class="folder-manager-container" fluid>
        <v-row>
            <v-col cols="12" md="9">
                <div class="files-preview-grid" @dragover.prevent="onDragOver" @drop.prevent="onDrop">
                    <div v-for="(file, idx) in uploadedFiles" :key="file.isServerFile ? file.id : 'local-' + idx" class="preview-item">
                        <img v-if="file.previewUrl && file.type.startsWith('image/')" :src="file.previewUrl"
                            class="preview-img" :alt="file.name" />
                        <v-icon v-else size="100" color="primary" class="pdf-icon">mdi-file-pdf-box</v-icon>
                        <v-btn icon class="delete-btn" color="error" @click="removeFile(idx)">
                            <v-icon>mdi-close</v-icon>
                        </v-btn>
                        <!-- هنا الإضافة الجديدة: حقل تعديل اسم الملف فقط للملفات المحلية -->
                        <v-text-field v-if="!file.isServerFile" v-model="file.name" label="تعديل اسم الملف" dense
                            hide-details class="rename-input" @keydown.enter.prevent :disabled="uploading" />
                        <!-- عرض اسم الملف فقط للملفات من السيرفر (غير قابلة للتعديل) -->
                        <div v-else class="file-name mt-2">{{ file.name }}</div>
                    </div>
                </div>
            </v-col>

            <v-col cols="8" md="3" class="upload-sidebar">
                <div class="sidebar-box mt-6">
                    <v-btn class="upload-btn mt-3" block color="primary" variant="outlined" @click="openFileDialog">رفع
                        الملفات</v-btn>
                    <input ref="fileInput" type="file" multiple class="d-none" accept="image/*,application/pdf"
                        @change="onFilesSelected" />
                </div>

                <v-btn class="save-btn mt6" color="primary" :disabled="!uploadedFiles.length || uploading"
                    @click="saveAllFiles" :loading="uploading">حفظ</v-btn>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="ts" setup>
import { uploadUserFiles, getUserFiles, deleteUserFile } from "../api/folderManagerService";
import { useRoute } from "vue-router";

import { onMounted, onUnmounted, ref } from 'vue';

// Start Service API
const route = useRoute();
const userId = Number(route.params.id);

async function saveAllFiles() {
    uploading.value = true;
    try {
        const formData = new FormData();
        formData.append('PersonalDataId', userId.toString());

        const fileNames: string[] = [];

        uploadedFiles.value
            .filter(item => !item.isServerFile)
            .forEach((item) => {
                if (item.file) {
                    formData.append("files", item.file);
                    fileNames.push(item.name);
                }
            });
        formData.append('fileNames', JSON.stringify(fileNames));

        await uploadUserFiles(userId, formData);
        uploadedFiles.value = uploadedFiles.value.filter(f => f.isServerFile);
        await fetchUserServerFiles();

    } catch (e) {

    } finally {
        uploading.value = false;
    }
}
// End Service API

onMounted(() => {
    window.addEventListener('dragover', preventDefault, false);
    window.addEventListener('drop', preventDefault, false);

    fetchUserServerFiles();
});
onUnmounted(() => {
    window.removeEventListener('dragover', preventDefault, false);
    window.removeEventListener('drop', preventDefault, false);
});

function preventDefault(e: Event) {
    e.preventDefault();
}


interface UploadedFile {
    file?: File;
    previewUrl: string | null;
    type: string;
    name: string;
    id?: number;
    isServerFile?: boolean;
    filePath?: string;
}

async function fetchUserServerFiles() {
    try {
        const res = await getUserFiles(userId);
        const serverFiles = (res.data.data ?? []).map((doc: any) => {

            let type = "";
            if (/\.(jpg|jpeg|png|gif|bmp|webp)$/i.test(doc.fileName)) {
                type = "image/" + doc.fileName.split('.').pop();
            } else if (/\.pdf$/i.test(doc.fileName)) {
                type = "application/pdf";
            }

            return {
                id: doc.id,
                previewUrl: getImageUrl(doc.filePath),
                type,
                name: doc.fileName,
                isServerFile: true,
                filePath: doc.filePath
            } as UploadedFile;
        });


        uploadedFiles.value = [
            ...serverFiles,
            ...uploadedFiles.value.filter(f => !f.isServerFile)
        ];
    } catch (err) {

    }
}

function getImageUrl(path) {
    return `http://localhost:5089${path}`;
}

const uploadedFiles = ref<UploadedFile[]>([]);
const uploading = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

function openFileDialog() {
    fileInput.value?.click();
}

function onFilesSelected(event: Event) {
    const files = (event.target as HTMLInputElement).files;
    if (!files) return;
    handleFiles(files);
}

function onDragOver(event: DragEvent) {
    event.preventDefault();
}

function onDrop(event: DragEvent) {
    const files = event.dataTransfer?.files;
    if (!files) return;
    handleFiles(files);
}

function handleFiles(files: FileList) {
    for (const file of Array.from(files)) {
        if (
            uploadedFiles.value.some(
                (f) => f.name === file.name && f.file.size === file.size
            )
        )
            continue;

        let previewUrl: string | null = null;
        if (file.type.startsWith("image/")) {
            previewUrl = URL.createObjectURL(file);
        }
        uploadedFiles.value.push({
            file,
            previewUrl,
            type: file.type,
            name: file.name,
        });
    }
}

async function removeFile(idx: number) {
    const file = uploadedFiles.value[idx];
    if (file.isServerFile && file.id) {

        await deleteUserFile(userId, file.id);
    }
    if (file.previewUrl && !file.isServerFile) {

        URL.revokeObjectURL(file.previewUrl);
    }
    uploadedFiles.value.splice(idx, 1);
}


</script>

<style scoped>
.folder-manager-container {
    background: #70a4cf34;
    border-radius: 12px;
    padding: 22px;
    min-height: 700px;
}

.files-preview-grid {
    display: flex;
    flex-wrap: wrap;
    gap: 28px;
    min-height: 360px;
    background: transparent;
}

.preview-item {
    position: relative;
    width: 220px;
    height: 260px;
    box-shadow: 0 2px 8px #0001;
    border-radius: 12px;
    overflow: hidden;
    background: #fff;
    display: flex;
    align-items: center;
    justify-content: center;
}

.preview-img {
    width: 100%;
    height: 100%;
    object-fit: contain;
}

.pdf-icon {
    margin: auto;
}

.delete-btn {
    position: absolute;
    top: 5px;
    left: 5px;
    background: #f44336dd;
    color: #fff;
    z-index: 2;
    border-radius: 50%;
}

.upload-sidebar {
    background: #b2d1f0ee;
    border-radius: 12px;
    padding: 34px 18px 18px 18px;
    min-height: 97vh;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.sidebar-box {
    width: 100%;
    border-radius: 6px;
    padding: 12px 0;
    background: #e9f3fb;
    font-size: 1.3rem;
    font-weight: 700;
    color: #163960;
    text-align: center;
    margin-bottom: 28px;
    letter-spacing: 0.5px;
}

.upload-btn {
    font-size: 1.2rem;
    font-weight: 500;
    padding: 12px 0 !important;
    background: #fff !important;
    color: #222 !important;
}

.save-btn {
    font-size: 1.3rem;
    font-weight: bold;
    border: 1.5px solid #222 !important;
    border-radius: 7px;
    background: #e1eefa !important;
    color: #0d2e4b !important;
    margin-top: auto;
    margin-bottom: 20px;
    width: 100%;
}

.section-label {
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 18px;
    display: block;
    color: #333;
    text-align: right;
}

.drop-area {
    width: 90%;
    min-height: 180px;
    background: #f5f8fa;
    border: 2px dashed #70b2b2;
    border-radius: 16px;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    margin-bottom: 36px;
    transition: border-color 0.22s;
    gap: 10px;
}

.drop-area:hover {
    border-color: #3f51b5;
}

.rename-input {
    position: absolute;
    bottom: 10px;
    width: 90%;
    left: 5%;
    background: #f6faff;
    border-radius: 6px;
}
.file-name {
    position: absolute;
    bottom: 8px;
    left: 0;
    width: 100%;
    text-align: center;
    font-weight: bold;
    font-size: 1.1rem;
    color: #163960;
    background: #f4f8ffb0;
    border-radius: 6px;
    padding: 4px 0;
}

</style>
