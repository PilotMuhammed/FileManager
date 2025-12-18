<template>
    <v-dialog :model-value="modelValue" @update:modelValue="emit('update:modelValue', $event)" max-width="800">
        <v-card class="pa-4 rounded-xl card-dialog">
            <v-btn class="close-btn" color="" icon variant="text" @click="handleClose">
                <v-icon>mdi-close</v-icon>
            </v-btn>

            <div class="d-flex justify-center align-center mb-4">
                <h3 class="text-h5 font-weight-bold mb-0">اضافة بيانات</h3>
            </div>

            <v-form @submit.prevent="handleSave">
                <v-row dense>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.fullName" hide-details
                            variant="outlined" density="comfortable" label="الاسم الرباعي" required outlined />
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.motherName" hide-details
                            variant="outlined" density="comfortable" label="اسم الأم الرباعي" required outlined />
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.birthDate" hide-details
                            variant="outlined" density="comfortable" label="المواليد" required outlined type="date" />
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.retirementNumber"
                            hide-details variant="outlined" density="comfortable" label="الرقم التقاعدي" outlined
                            type="number" />
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.deliveredTo" hide-details
                            variant="outlined" density="comfortable" label="مسلمة إلى" outlined />
                    </v-col>
                    <v-col cols="12" md="6">
                        <v-text-field dir="rtl" class="rounded-xl text-field" v-model="form.receivedFrom" hide-details
                            variant="outlined" density="comfortable" label=" مستلمة من" outlined />
                    </v-col>
                </v-row>
                <div class="d-flex justify-center mt-10">
                    <v-btn color="primary" type="submit" class="ms-2">حفظ</v-btn>
                </div>
            </v-form>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from "vue";

const props = defineProps<{
    modelValue: boolean;
    loading: boolean;
    errorMsg?: string;
    initialData?: any;
}>();
const emit = defineEmits(["update:modelValue", "save", "close"]);


const form = ref({
    fullName: "",
    motherName: "",
    birthDate: "",
    retirementNumber: "",
    deliveredTo: "",
    receivedFrom: "",
});


watch(
    () => props.modelValue,
    (val) => {
        if (!val) {
            form.value = {
                fullName: "",
                motherName: "",
                birthDate: "",
                retirementNumber: "",
                deliveredTo: "",
                receivedFrom: "",
            };
        }
    }
);


function handleClose() {
    emit("update:modelValue", false);
    emit("close");
}


function handleSave() {
    emit("save", { ...form.value });
}
</script>

<style scoped>
.card-dialog {
    overflow: visible !important;
    position: relative;
}

.close-btn {
    position: absolute;
    top: -15px;
    left: -15px;
    background-color: var(--primary);
}

.close-btn:hover {
    background-color: var(--alarm);
    color: white;
}

.text-field {
    direction: rtl;
}
</style>
