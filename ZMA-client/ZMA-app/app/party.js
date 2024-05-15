import { StyleSheet, View, Text, FlatList, SafeAreaView, Pressable } from 'react-native';
import { useState, useEffect } from 'react';
import { useNavigation } from "expo-router";

import { COLORS } from '../constants/theme';
import PartyCard from '../components/partyCard';

import Logo from '../components/logo';

export default function PartyManager() {
    const [parties, setParties] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigation = useNavigation();

    async function fetchParties() {
        const res = await fetch(`http://localhost:5086/Party/GetParties`,
            {
                method: "GET",
                credentials: 'include',
                headers: { 'Content-type': 'application/json' }
            }
        );
        return await res.json();
    }

    useEffect(() => {
        fetchParties()
            .then(parties => {
                setParties(parties);
                setLoading(false);
            })
    }, []);

    if (loading) return <View><Text>Loading...</Text></View>

    return (
        <SafeAreaView style={styles.safeArea}>
            <Logo />
            <View style={styles.header}>
                <Text style={styles.main}>Bulik</Text>
            </View>
            <Pressable style={[styles.button, { backgroundColor: '#9c27b0' }]}>
                <Text style={styles.buttonText}>Buli létrehozás</Text>
            </Pressable>
            <FlatList
                data={parties}
                renderItem={({ item }) => <PartyCard party={item} onPress={() => navigation.navigate('songRequests', { id: item.id } )} />}
                keyExtractor={item => item.id}
                contentContainerStyle={styles.listContent}
            />
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    safeArea: {
      flex: 1,
      backgroundColor: '#fff',
    },
    header: {
      padding: 16,
      backgroundColor: COLORS.deepPurple,
      alignItems: 'center',
    },
    main: {
      fontSize: 24,
      fontWeight: 'bold',
      color: '#fff',
    },
    listContent: {
      padding: 16,
    },
    button: {
        justifyContent: 'center',
        alignItems: 'center',
        paddingVertical: 12,
        paddingHorizontal: 24,
        borderRadius: 8,
        elevation: 2,
    },
    buttonText: {
        fontSize: 16,
        fontWeight: 'bold',
        color: '#FFFFFF',
    },
  });