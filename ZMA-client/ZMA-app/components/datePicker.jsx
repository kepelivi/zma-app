import DatePicker from 'react-native-neat-date-picker';
import { useState } from 'react';
import { View, Pressable, Text, StyleSheet, Modal } from 'react-native';

import { COLORS } from '../constants/theme';

export default function DateSetter({ date, setDate }) {
    const [showDatePickerSingle, setShowDatePickerSingle] = useState(false);
  
    const openDatePickerSingle = () => setShowDatePickerSingle(true);
  
    const onCancelSingle = () => {
      setShowDatePickerSingle(false);
    };
  
    const onConfirmSingle = (output) => {
      setShowDatePickerSingle(false);
      setDate(output.dateString);
    };
  
    return (
      <View style={styles.container}>
        <Pressable onPress={openDatePickerSingle} style={styles.dateButton}>
          <Text style={styles.dateButtonText}>Válasszon dátumot</Text>
        </Pressable>
        <Modal
          transparent={true}
          visible={showDatePickerSingle}
          animationType="slide"
        >
          <View style={styles.modalContainer}>
            <DatePicker
              isVisible={showDatePickerSingle}
              mode={'single'}
              onCancel={onCancelSingle}
              onConfirm={onConfirmSingle}
              colorOptions={{
                headerColor: COLORS.deepPurple,
                backgroundColor: COLORS.white,
                selectedDateColor: COLORS.purple,
                confirmButtonColor: COLORS.purple,
              }}
            />
          </View>
        </Modal>
        <Text style={styles.dateText}>{date}</Text>
      </View>
    );
  }
  
  const styles = StyleSheet.create({
    container: {
      justifyContent: 'center',
      alignItems: 'center',
    },
    dateButton: {
      backgroundColor: COLORS.deepPurple,
      padding: 10,
      borderRadius: 8,
      marginTop: 8,
    },
    dateButtonText: {
      color: COLORS.white,
      fontSize: 16,
    },
    modalContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },
    dateText: {
      marginTop: 8,
      color: COLORS.deepPurple,
    },
  });