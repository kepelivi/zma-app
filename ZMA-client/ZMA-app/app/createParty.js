import { View, Text, TextInput, Pressable, StyleSheet, SafeAreaView } from "react-native";
import { useNavigation } from "expo-router";
import { useState } from "react";
import { notification } from "antd";

import { COLORS } from "../constants/theme";
import { apiUrl } from "../constants/config";
import Logo from "../components/logo";
import GoBack from "../components/back";

notification.config({
    duration: 2,
    closeIcon: null
})

export default function createParty() {
    const [name, setName] = useState("");
    const [details, setDetails] = useState("");
    const [category, setCategory] = useState("");
    const [date, setDate] = useState("");

    const navigation = useNavigation();

    async function handleCreate() {
        const res = await fetch(`${apiUrl}Party/CreateParty?name=${name}&details=${details}&category=${category}&date=${date}`, {
            method: 'POST',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' }
        })
        if (!res.ok) {
            notification.error({ message: "Buli létrehozása sikertelen." })
        }
        navigation.navigate('party');
    }


    return (
        <SafeAreaView style={{ flex: 1, backgroundColor: COLORS.white }}>
            <GoBack />
            <Logo />
            <View style={styles.container}>
                <View style={styles.inputContainer}>
                    <TextInput
                        style={styles.input}
                        placeholder="Buli neve"
                        value={name}
                        onChangeText={setName}
                        autoCorrect={false}
                        autoCapitalize="none"
                        placeholderTextColor={COLORS.greyish}
                    />
                    <TextInput
                        style={styles.input}
                        placeholder="Részletek"
                        value={details}
                        onChangeText={setDetails}
                        autoCorrect={false}
                        autoCapitalize="none"
                        placeholderTextColor={COLORS.greyish}
                    />
                    <TextInput
                        style={styles.input}
                        placeholder="Kategória (esküvő, szalagavató, stb...)"
                        value={category}
                        onChangeText={setCategory}
                        autoCorrect={false}
                        autoCapitalize="none"
                        placeholderTextColor={COLORS.greyish}
                    />
                    <Pressable style={styles.button} onPress={handleCreate}>
                        <Text style={styles.buttonText}>Mentés</Text>
                    </Pressable>
                </View>
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
      width: '80%',
      alignItems: 'center',
    },
    input: {
      width: '100%',
      paddingVertical: 8,
      paddingHorizontal: 16,
      borderWidth: 1,
      borderColor: COLORS.greyish,
      borderRadius: 8,
      marginBottom: 16,
      backgroundColor: COLORS.white,
    },
    label: {
      marginBottom: 8,
      color: COLORS.deepPurple,
    },
    button: {
      justifyContent: 'center',
      alignItems: 'center',
      paddingVertical: 12,
      paddingHorizontal: 24,
      borderRadius: 8,
      backgroundColor: COLORS.purple,
      marginTop: 16,
    },
    buttonText: {
      color: COLORS.white,
      fontSize: 16,
    },
  });