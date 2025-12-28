<template>
    <v-dialog v-model="visible" max-width="600">
        <v-card>
            <v-card-title class="text-h6">
                تعديل الصورة
            </v-card-title>
            <v-card-text>
                <div v-if="imageToEdit">
                    <cropper ref="cropperRef" :src="imageToEdit" :stencil-props="{ aspectRatio: 1 }" :autoZoom="true"
                        :default-size="{ width: 300, height: 300 }" class="cropper" />
                </div>
                <div class="actions mt-4">
                    <v-btn icon @click="rotate"><v-icon>mdi-rotate-right</v-icon></v-btn>
                    <v-btn icon @click="resetCrop"><v-icon>mdi-crop-free</v-icon></v-btn>
                </div>
            </v-card-text>
            <v-card-actions>
                <v-spacer />
                <v-btn color="primary" @click="handleSave">حفظ</v-btn>
                <v-btn color="grey" @click="emitClose">إلغاء</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from "vue";
import { Cropper } from "vue-advanced-cropper";
import "vue-advanced-cropper/dist/style.css";

const props = defineProps<{
    modelValue: boolean;
    image: string | null;
}>();
const emits = defineEmits(["update:modelValue", "save"]);

const visible = ref(props.modelValue);
const imageToEdit = ref<string | null>(props.image || null);
const cropperRef = ref<any>(null);
const rotation = ref(0);


watch(() => props.modelValue, (val) => visible.value = val);
watch(() => props.image, (val) => imageToEdit.value = val);


function emitClose() {
    emits("update:modelValue", false);
}

function rotate() {
    rotation.value = (rotation.value + 90) % 360;
    if (cropperRef.value) {
        cropperRef.value.rotate(90);
    }
}

function resetCrop() {
    if (cropperRef.value) {
        cropperRef.value.reset();
    }
}

async function handleSave() {
    if (!cropperRef.value) return;
    const canvas = cropperRef.value.getResult().canvas;
    if (!canvas) return;

    canvas.toBlob((blob: Blob | null) => {
        if (blob) {
            const url = URL.createObjectURL(blob);
            emits("save", { url, blob });
            emits("update:modelValue", false);
        }
    }, "image/jpeg", 0.95);
}
</script>

<style scoped>
.cropper {
    width: 100%;
    height: 350px;
    background: #f3f6fa;
    margin: 0 auto 8px auto;
}

.actions {
    display: flex;
    justify-content: center;
    gap: 12px;
}
</style>
