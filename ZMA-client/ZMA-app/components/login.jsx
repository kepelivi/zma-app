import {
    Text,
    TextInput,
    View,
    SafeAreaView,
    StyleSheet,
    Pressable
} from "react-native";
import { useState } from "react";
import { useNavigation } from "expo-router";
import { notification } from "antd";
import { apiUrl } from "../constants/config";
import LoginEye from "./loginEye";

notification.config({
    duration: 2,
    closeIcon: null
})

import { COLORS } from "../constants/theme";

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const navigation = useNavigation();

    const toggleShowPassword = () => {
        setShowPassword(!showPassword);
    };

    async function handleLogin(email, password) {
        styles.button.backgroundColor = "black";
        try {
            const res = await fetch(`${apiUrl}Auth/Login`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: email, Password: password })
            })
            if (!res.ok) {
                notification.error({ message: "E-mail vagy jelszó hibás. Kérem próbálja újra!" })
                throw new Error('Invalid credentials. Please try again.');
            }
            notification.success({ message: "Sikeres bejelentkezés!" });
            navigation.navigate('party');
        } catch (error) {
            
        }
    }

    return (
        <SafeAreaView style={{ flex: 1, backgroundColor: 'white' }}>
            <View style={styles.container}>
                <View style={styles.inputContainer}>
                    <TextInput
                        style={styles.input}
                        placeholder="EMAIL"
                        value={email}
                        onChangeText={setEmail}
                        autoCorrect={false}
                        autoCapitalize="none"
                        placeholderTextColor={COLORS.greyish}
                    />
                    <View style={styles.passwordContainer}>
                        <TextInput
                            style={styles.passwordInput}
                            secureTextEntry={!showPassword}
                            placeholder="JELSZÓ"
                            value={password}
                            onChangeText={setPassword}
                            autoCorrect={false}
                            autoCapitalize="none"
                            placeholderTextColor={COLORS.greyish}
                        />
                        <Pressable
                            size={24}
                            onPress={toggleShowPassword}
                            accessibilityLabel='toggle-password-visibility'
                        >
                            <LoginEye showPassword={showPassword} />
                        </Pressable>
                    </View>
                </View>
                <Pressable style={styles.button} onPress={() => handleLogin(email, password)}>
                    <Text style={styles.buttonText}>Belépés</Text>
                </Pressable>
            </View>
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    inputContainer: {
        marginBottom: 20,
    },
    input: {
        width: 300,
        height: 40,
        borderWidth: 1,
        borderColor: COLORS.greyish,
        borderRadius: 5,
        paddingLeft: 10,
        marginBottom: 10,
        color: 'black',
        paddingRight: 40,
    },
    button: {
        backgroundColor: '#9c27b0',
        paddingVertical: 10,
        paddingHorizontal: 20,
        borderRadius: 5,
        zIndex: 1000,
    },
    buttonText: {
        color: 'white',
        fontWeight: 'bold',
        fontSize: 16,
    },
    passwordContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        position: 'relative',
        width: '100%',
    },
    passwordInput: {
        flex: 1,
        height: 40,
        borderWidth: 1,
        borderColor: COLORS.greyish,
        borderRadius: 5,
        paddingLeft: 10,
        color: 'black',
    },
    icon: {
        position: 'absolute',
        right: 10,
        color: COLORS.greyish
    },
});

export default Login;