<script setup>
import axios from 'axios'
import { ref } from "vue"

const isAuth = ref(false)
const log = ref("")
const password = ref("")
const currentUser = ref(null)
const users = ref([])
const errorMessage = ref("")
const successMessage = ref("")
const showRegistration = ref(false)

const regLogin = ref("")
const regPassword = ref("")
const regName = ref("")
const regEmail = ref("")
const regPhone = ref("")

const apiLogin = async (login, password) => {
  const response = await axios.post('http://localhost:3000/api/Users/auth', {
    Login: login,
    Password: password
  })
  return response.data
}

const apiGetUsers = async () => {
  const response = await axios.get('http://localhost:3000/api/Users')
  return response.data
}

const apiRegister = async (userData) => {
  const response = await axios.post('http://localhost:3000/api/Users/register', userData)
  return response.data
}

const login = async () => {
  try {
    errorMessage.value = ""
    successMessage.value = ""

    const userData = await apiLogin(log.value, password.value)
    currentUser.value = userData
    isAuth.value = true

    if (currentUser.value.idRole === 1) {
      users.value = await apiGetUsers()
    }
  } catch (error) {
    errorMessage.value = "Ошибка авторизации: неверный логин или пароль"
  }
}

const register = async () => {
  try {
    errorMessage.value = ""
    successMessage.value = ""
    if (regEmail.value && !isValidEmail(regEmail.value)) {
      errorMessage.value = "Введите корректный email адрес"
      return
    }

    if (regPhone.value && !isValidPhone(regPhone.value)) {
      errorMessage.value = "Введите корректный номер телефона"
      return
    }

    const userData = {
      Login: regLogin.value,
      Password: regPassword.value,
      Name: regName.value,
      Email: regEmail.value || null,
      PhoneNumber: regPhone.value || null
    }

    const result = await apiRegister(userData)
    successMessage.value = "Регистрация прошла успешно! Теперь вы можете войти."
    
    clearRegistrationForm()
    showRegistration.value = false
  } catch (error) {
    if (error.response?.status === 409) {
      errorMessage.value = "Пользователь с таким логином уже существует"
    } else if (error.response?.status === 405) {
      errorMessage.value = "Метод не допущен"
    } else if (error.response?.status === 400) {
      errorMessage.value = "Заполните все обязательные поля"
    } else {
      errorMessage.value = "Ошибка при регистрации. Попробуйте позже."
    }
  }
}

const isValidEmail = (email) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(email)
}

const isValidPhone = (phone) => {
  const cleanPhone = phone.replace(/[^\d+]/g, '')
  const phoneRegex = /^\+7\d{10}$/
  return phoneRegex.test(cleanPhone)
}

const formatPhone = (event) => {
  let value = event.target.value.replace(/\D/g, '')
  
  if (value.length === 0) return ""
  
  if (!value.startsWith('7') && value.length > 0) {
    value = '7' + value
  }
  
  let formattedValue = '+7'
  
  if (value.length > 1) {
    formattedValue += ' ' + value.slice(1, 4)
  }
  if (value.length > 4) {
    formattedValue += ' ' + value.slice(4, 7)
  }
  if (value.length > 7) {
    formattedValue += ' ' + value.slice(7, 9)
  }
  if (value.length > 9) {
    formattedValue += ' ' + value.slice(9, 11)
  }
  
  regPhone.value = formattedValue
}

const formatEmail = (event) => {
  let value = event.target.value.toLowerCase()
  
  if (value.includes('@') && !value.includes('.') && !value.endsWith('@')) {
    const [localPart] = value.split('@')
    const commonDomains = ['gmail.com', 'mail.ru', 'yandex.ru', 'rambler.ru']
    
    regEmail.value = value
  } else {
    regEmail.value = value
  }
}

const logout = () => {
  isAuth.value = false
  currentUser.value = null
  users.value = []
  log.value = ""
  password.value = ""
  errorMessage.value = ""
  successMessage.value = ""
}

const clearRegistrationForm = () => {
  regLogin.value = ""
  regPassword.value = ""
  regName.value = ""
  regEmail.value = ""
  regPhone.value = ""
}

const toggleRegistration = () => {
  showRegistration.value = !showRegistration.value
  errorMessage.value = ""
  successMessage.value = ""
  clearRegistrationForm()
}

const clearPhoneOnFocus = () => {
  if (regPhone.value === '+7') {
    regPhone.value = ''
  }
}

const restorePhoneFormat = () => {
  if (regPhone.value && !regPhone.value.startsWith('+7')) {
    formatPhone({ target: { value: regPhone.value } })
  }
}
</script>

<template>
  <div class="container">
    <header>
      <h3>Докер Веб-клиент</h3>
      <div v-if="isAuth" class="user-info">
        <span>{{ currentUser?.login }}</span>
        <button @click="logout">Выйти</button>
      </div>
    </header>
    <main>
      <div v-if="!isAuth" class="auth-section">
        <div v-if="!showRegistration" class="auth">
          <h4>Авторизация</h4>
          <input v-model="log" placeholder="Логин" />
          <input v-model="password" type="password" placeholder="Пароль" />
          <button @click="login">Войти</button>
          <button @click="toggleRegistration" class="secondary-btn">Регистрация</button>
          
          <div v-if="errorMessage" class="error">{{ errorMessage }}</div>
          <div v-if="successMessage" class="success">{{ successMessage }}</div>
        </div>

        <div v-else class="registration">
          <h4>Регистрация</h4>
          <input v-model="regLogin" placeholder="Логин *" required />
          <input v-model="regPassword" type="password" placeholder="Пароль *" required />
          <input v-model="regName" placeholder="Имя *" required />
          <input 
            v-model="regEmail" 
            type="email" 
            placeholder="example@mail.ru"
            @input="formatEmail"
            @blur="regEmail && !isValidEmail(regEmail) ? errorMessage = 'Введите корректный email' : ''"
          />
          <input 
            v-model="regPhone" 
            placeholder="+7 919 159 42 50"
            @input="formatPhone"
            @focus="clearPhoneOnFocus"
            @blur="restorePhoneFormat"
            maxlength="16"
          />
          
          <div class="form-notes">
            <small>Поля помеченные * обязательны для заполнения</small><br>
            <small>Формат телефона: +7 XXX XXX XX XX</small><br>
            <small>Пример email: example@mail.ru</small><br>
            <small>Почта и логин должны быть более 8 символов</small>
          </div>
          
          <button @click="register" class="register-btn">Зарегистрироваться</button>
          <button @click="toggleRegistration" class="secondary-btn">Назад к авторизации</button>
          
          <div v-if="errorMessage" class="error">{{ errorMessage }}</div>
          <div v-if="successMessage" class="success">{{ successMessage }}</div>
        </div>
      </div>
      
      <div v-else>
        <div v-if="currentUser.idRole === 1">
          <h4>Пользователи</h4>
          <table>
            <thead>
            <tr>
              <th>ID</th>
              <th>Логин</th>
              <th>Имя</th>
              <th>Email</th>
              <th>Телефон</th>
              <th>Роль</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="user in users" :key="user.idUser">
              <td>{{ user.idUser }}</td>
              <td>{{ user.login }}</td>
              <td>{{ user.name }}</td>
              <td>{{ user.email || '-' }}</td>
              <td>{{ user.phoneNumber || '-' }}</td>
              <td>{{ user.idRole === 1 ? 'Админ' : 'Клиент' }}</td>
            </tr>
            </tbody>
          </table>
        </div>
        <div v-else>
          <h4>Ваши данные</h4>
          <div class="user-data">
            <div><strong>ID:</strong> {{ currentUser.idUser }}</div>
            <div><strong>Логин:</strong> {{ currentUser.login }}</div>
            <div><strong>Имя:</strong> {{ currentUser.name }}</div>
            <div v-if="currentUser.email"><strong>Email:</strong> {{ currentUser.email }}</div>
            <div v-if="currentUser.phoneNumber"><strong>Телефон:</strong> {{ currentUser.phoneNumber }}</div>
            <div><strong>Роль:</strong> {{ currentUser.idRole === 1 ? 'Админ' : 'Клиент' }}</div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped>
h3 {
  color: black;
}
h4 {
  color: black;
}
th {
  color: black;
}
td {
  color: black;
}
strong {
  color: black;
}

.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding-bottom: 10px;
  border-bottom: 1px solid #ddd;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.auth-section {
  max-width: 400px;
  margin: 0 auto;
}

.auth, .registration {
  width: 100%;
}

.auth h4, .registration h4 {
  margin-bottom: 15px;
  text-align: center;
}

input {
  width: 100%;
  padding: 10px;
  margin-bottom: 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-sizing: border-box;
  font-family: monospace;
}

input::placeholder {
  color: #999;
  font-family: inherit;
}

button {
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 10px;
  margin-bottom: 10px;
}

button:not(.secondary-btn) {
  background: #007bff;
  color: white;
}

button:hover:not(.secondary-btn) {
  background: #0056b3;
}

.secondary-btn {
  background: #6c757d;
  color: white;
}

.secondary-btn:hover {
  background: #545b62;
}

.register-btn {
  background: #28a745;
}

.register-btn:hover {
  background: #218838;
}

.error {
  color: #dc3545;
  margin-top: 10px;
  padding: 8px;
  background: #f8d7da;
  border: 1px solid #f5c6cb;
  border-radius: 4px;
}

.success {
  color: #155724;
  margin-top: 10px;
  padding: 8px;
  background: #d4edda;
  border: 1px solid #c3e6cb;
  border-radius: 4px;
}

.form-notes {
  margin-bottom: 15px;
  line-height: 1.4;
}

.form-notes small {
  color: #6c757d;
  font-size: 0.85em;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

th, td {
  padding: 10px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background: #f5f5f5;
  font-weight: bold;
}

.user-data div {
  margin-bottom: 10px;
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.user-data strong {
  display: inline-block;
  width: 100px;
}

input[placeholder="+7 919 159 42 50"] {
  font-family: 'Courier New', monospace;
  letter-spacing: 0.5px;
}

input[placeholder="example@mail.ru"] {
  font-family: 'Courier New', monospace;
}
</style>