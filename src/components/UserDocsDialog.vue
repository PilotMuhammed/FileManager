<template>
    <v-dialog :model-value="modelValue" @update:modelValue="emit('update:modelValue', $event)" max-width="900">
        <v-card>
            <v-card-title>
                <span class="font-weight-bold">وثائق المستخدم: {{ userName }}</span>
            </v-card-title>
            <v-divider />
            <v-card-text>
                <div v-if="userDocs.length > 0" class="docs-grid">
                    <div v-for="doc in userDocs" :key="doc.id" class="doc-item">
                        <template v-if="isImage(doc.fileName)">
                            <a :href="doc.filePath" target="_blank">
                                <img :src="getImageUrl(doc.filePath)" class="doc-thumb" loading="lazy" />
                            </a>
                            <div class="doc-name">{{ doc.fileName }}</div>
                            <div class="doc-date">{{ doc.uploadedAt?.replace('T', ' ').substring(0, 19) }}</div>
                        </template>
                        <template v-else>
                            <a :href="doc.filePath" target="_blank">
                                <v-icon color="primary" size="40">mdi-file-document-outline</v-icon>
                                <div class="doc-name">{{ doc.fileName }}</div>
                            </a>
                            <div class="doc-date">{{ doc.uploadedAt?.replace('T', ' ').substring(0, 19) }}</div>
                        </template>
                    </div>
                </div>
                <div v-else>
                    لا توجد وثائق.
                </div>
            </v-card-text>
            <v-card-actions>
                <v-btn color="primary" @click="handleClose">إغلاق</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

const props = defineProps<{
    modelValue: boolean,
    userDocs: any[],
    userName: string,
}>();
const emit = defineEmits(['update:modelValue']);

function handleClose() {
    emit('update:modelValue', false);
}

function isImage(fileName: string) {
    return /\.(jpg|jpeg|png|gif|bmp|webp)$/i.test(fileName);
}
function getImageUrl(path: string) {
    return `http://localhost:5089${path}`;
}
</script>

<style scoped>
.docs-grid {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
    justify-content: start;
    align-items: flex-start;
}

.doc-item {
    width: 180px;
    text-align: center;
    margin-bottom: 20px;
}

.doc-thumb {
    width: 160px;
    height: 110px;
    object-fit: contain;
    border: 2px solid #eee;
    border-radius: 12px;
    margin-bottom: 8px;
    background: #f8f9fa;
    box-shadow: 0 2px 6px 0 #eee;
}

.doc-name {
    font-size: 15px;
    font-weight: bold;
    color: #1a1a1a;
    word-break: break-word;
}

.doc-date {
    font-size: 13px;
    color: #888;
}
</style>
